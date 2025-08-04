// GenericEditForm.cs
using System.Net.Http.Json;
using System.Reflection;

namespace FrontendWindowsForm.Features
{
    public partial class GenericEditForm : Form
    {
        public event EventHandler<object> ItemSaved; // Cambiado de ItemUpdated a ItemSaved

        private HttpClient _client;
        private object _item; // Puede ser null para creación
        private string _apiEndpoint;
        private Dictionary<string, Control> _editControls;
        private List<string> _readOnlyProperties;
        private Type _itemType;
        private bool _isCreation; // Nuevo: indica si es creación

        // Constructor para Edición
        public GenericEditForm(HttpClient client, object item, string apiEndpoint, List<string> readOnlyProperties = null)
        {
            InitializeComponent();
            _client = client;
            _item = item;
            _apiEndpoint = apiEndpoint;
            _readOnlyProperties = readOnlyProperties ?? new List<string>();
            _itemType = item.GetType();
            _isCreation = false; // Es edición
            _editControls = new Dictionary<string, Control>();

            this.Text = $"Editar {_itemType.Name}";
            InitializeForm();
        }

        // Constructor para Creación
        public GenericEditForm(HttpClient client, Type itemType, string apiEndpoint, List<string> readOnlyProperties = null)
        {
            InitializeComponent();
            _client = client;
            _item = null; // No hay item existente
            _apiEndpoint = apiEndpoint;
            _readOnlyProperties = readOnlyProperties ?? new List<string>();
            _itemType = itemType;
            _isCreation = true; // Es creación
            _editControls = new Dictionary<string, Control>();

            this.Text = $"Crear {_itemType.Name}";
            InitializeForm();
        }

        private void InitializeForm()
        {
            int top = 20;
            int labelWidth = 100;
            int controlWidth = 200;
            int controlHeight = 25;
            int verticalSpacing = 35;

            // Obtener propiedades editables
            var properties = _itemType.GetProperties()
                .Where(p => p.CanRead && p.CanWrite && p.GetSetMethod() != null);

            foreach (var prop in properties)
            {
                // Saltar propiedades de navegación o complejas
                if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                    continue;

                // Crear label
                var label = new Label
                {
                    Text = prop.Name,
                    Location = new Point(20, top),
                    Size = new Size(labelWidth, controlHeight),
                    TextAlign = ContentAlignment.MiddleRight
                };

                // Crear control de edición
                Control editControl = CreateEditControl(prop);
                editControl.Location = new Point(130, top);
                editControl.Size = new Size(controlWidth, controlHeight);

                // Para creación, todas las propiedades son editables a menos que estén en readOnly
                bool isReadOnly = _isCreation ? _readOnlyProperties.Contains(prop.Name) : _readOnlyProperties.Contains(prop.Name);
                editControl.Enabled = !isReadOnly;

                // Si es edición y hay un item, cargar el valor
                if (!_isCreation && _item != null)
                {
                    var value = prop.GetValue(_item);
                    SetControlValue(editControl, value, prop.PropertyType);
                }

                _editControls[prop.Name] = editControl;

                this.Controls.Add(label);
                this.Controls.Add(editControl);

                top += verticalSpacing;
            }

            // Botones
            var btnSave = new Button
            {
                Text = _isCreation ? "Crear" : "Guardar",
                Location = new Point(80, top + 10),
                Size = new Size(80, 30)
            };
            btnSave.Click += BtnSave_Click;

            var btnCancel = new Button
            {
                Text = "Cancelar",
                Location = new Point(180, top + 10),
                Size = new Size(80, 30)
            };
            btnCancel.Click += BtnCancel_Click;

            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);

            // Ajustar tamaño del formulario
            this.Size = new Size(380, top + 100);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private Control CreateEditControl(PropertyInfo prop)
        {
            if (prop.PropertyType == typeof(bool))
            {
                return new CheckBox();
            }
            else if (prop.PropertyType.IsEnum)
            {
                var comboBox = new ComboBox();
                comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox.Items.AddRange(Enum.GetNames(prop.PropertyType));
                return comboBox;
            }
            else
            {
                var textBox = new TextBox();

                // Validaciones básicas
                if (prop.Name.ToLower().Contains("password"))
                {
                    textBox.PasswordChar = '*';
                }
                else if (prop.Name.ToLower().Contains("email"))
                {
                    // Podrías agregar validación visual aquí si lo deseas
                }

                return textBox;
            }
        }

        private void SetControlValue(Control control, object value, Type propertyType)
        {
            if (control is TextBox textBox)
            {
                textBox.Text = value?.ToString() ?? "";
            }
            else if (control is CheckBox checkBox)
            {
                checkBox.Checked = value?.ToString().ToLower() == "true";
            }
            else if (control is ComboBox comboBox)
            {
                comboBox.SelectedItem = value?.ToString();
            }
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear instancia del objeto a guardar
                var itemToSave = Activator.CreateInstance(_itemType);

                foreach (var kvp in _editControls)
                {
                    var propName = kvp.Key;
                    var control = kvp.Value;
                    var prop = _itemType.GetProperty(propName);

                    if (prop != null)
                    {
                        object value = null;

                        if (control is TextBox textBox)
                            value = ConvertValue(textBox.Text, prop.PropertyType);
                        else if (control is CheckBox checkBox)
                            value = checkBox.Checked;
                        else if (control is ComboBox comboBox)
                            value = comboBox.SelectedItem != null ? Enum.Parse(prop.PropertyType, comboBox.SelectedItem.ToString()) : null;

                        prop.SetValue(itemToSave, value);
                    }
                }

                HttpResponseMessage response;
                if (_isCreation)
                {
                    // Para creación, usar POST
                    response = await _client.PostAsJsonAsync(_apiEndpoint, itemToSave);
                }
                else
                {
                    // Para edición, usar PUT (asumiendo que el endpoint ya incluye el ID)
                    response = await _client.PutAsJsonAsync(_apiEndpoint, itemToSave);
                }

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show(_isCreation ? "Registro creado correctamente" : "Datos actualizados correctamente");
                    ItemSaved?.Invoke(this, itemToSave); // Disparar evento con el item creado/guardado
                    this.Close();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error: {error}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private object ConvertValue(string value, Type targetType)
        {
            if (string.IsNullOrEmpty(value))
            {
                // Para tipos valor, devolver valor por defecto; para referencias, null
                return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
            }

            try
            {
                if (targetType == typeof(string)) return value;
                if (targetType == typeof(int)) return int.Parse(value);
                if (targetType == typeof(decimal)) return decimal.Parse(value);
                if (targetType == typeof(double)) return double.Parse(value);
                if (targetType == typeof(DateTime)) return DateTime.Parse(value);
                if (targetType == typeof(bool)) return bool.Parse(value);

                return value;
            }
            catch
            {
                return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}