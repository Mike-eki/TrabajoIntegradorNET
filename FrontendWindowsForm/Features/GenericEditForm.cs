// GenericEditForm.cs
using System.Net.Http.Json;
using System.Reflection;

namespace FrontendWindowsForm.Features
{
    public partial class GenericEditForm : Form
    {
        public event EventHandler<object> ItemUpdated;

        private HttpClient _client;
        private object _item;
        private string _apiEndpoint;
        private Dictionary<string, Control> _editControls;
        private List<string> _readOnlyProperties;
        private Type _itemType;

        public GenericEditForm(HttpClient client, object item, string apiEndpoint, List<string> readOnlyProperties = null)
        {
            InitializeComponent();
            _client = client;
            _item = item;
            _apiEndpoint = apiEndpoint;
            _readOnlyProperties = readOnlyProperties ?? new List<string>();
            _itemType = item.GetType();
            _editControls = new Dictionary<string, Control>();

            this.Text = $"Editar {_itemType.Name}";
            InitializeForm();
        }

        private void InitializeForm()
        {
            int top = 20;
            int labelWidth = 100;
            int controlWidth = 200;
            int controlHeight = 25;
            int verticalSpacing = 35;

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
                editControl.Enabled = !_readOnlyProperties.Contains(prop.Name);

                _editControls[prop.Name] = editControl;

                this.Controls.Add(label);
                this.Controls.Add(editControl);

                top += verticalSpacing;
            }

            // Botones
            var btnSave = new Button
            {
                Text = "Guardar",
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
            var value = prop.GetValue(_item);

            if (prop.PropertyType == typeof(bool))
            {
                var checkBox = new CheckBox();
                checkBox.Checked = value?.ToString().ToLower() == "true";
                return checkBox;
            }
            else if (prop.PropertyType.IsEnum)
            {
                var comboBox = new ComboBox();
                comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox.Items.AddRange(Enum.GetNames(prop.PropertyType));
                comboBox.SelectedItem = value?.ToString();
                return comboBox;
            }
            else
            {
                var textBox = new TextBox();
                textBox.Text = value?.ToString() ?? "";

                // Validaciones básicas
                if (prop.Name.ToLower().Contains("email"))
                {
                    textBox.Validating += (s, e) => {
                        if (!string.IsNullOrEmpty(textBox.Text) &&
                            !textBox.Text.Contains("@"))
                        {
                            MessageBox.Show("Email inválido");
                        }
                    };
                }

                return textBox;
            }
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Actualizar objeto con valores del formulario
                var updatedItem = Activator.CreateInstance(_itemType);

                foreach (var kvp in _editControls)
                {
                    var propName = kvp.Key;
                    var control = kvp.Value;
                    var prop = _itemType.GetProperty(propName);

                    if (prop != null && !_readOnlyProperties.Contains(propName))
                    {
                        object value = null;

                        if (control is TextBox textBox)
                            value = ConvertValue(textBox.Text, prop.PropertyType);
                        else if (control is CheckBox checkBox)
                            value = checkBox.Checked;
                        else if (control is ComboBox comboBox)
                            value = Enum.Parse(prop.PropertyType, comboBox.SelectedItem?.ToString() ?? "");

                        prop.SetValue(updatedItem, value);
                    }
                    else
                    {
                        // Mantener valor original para propiedades de solo lectura
                        var originalValue = _itemType.GetProperty(propName)?.GetValue(_item);
                        prop?.SetValue(updatedItem, originalValue);
                    }
                }

                // Enviar actualización a la API
                var response = await _client.PutAsJsonAsync(_apiEndpoint, updatedItem);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Datos actualizados correctamente");
                    ItemUpdated?.Invoke(this, updatedItem);
                    this.Close();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al actualizar: {error}");
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