using System.Net.Http.Json;
using Models.Enums;
using DTOs;

namespace FrontendWindowsForm
{
    public partial class LoginForm : Form
    {
        private HttpClient _client;
        private ApplicationManager _appManager; // Referencia al gestor
        public LoginForm(ApplicationManager appManager)
        {
            InitializeComponent();
            _client = HttpClientManager.GetClient(); // ✅ Usar el compartido
            _appManager = appManager; // Recibir el gestor
        }
        public class LoginResponse
        {
            public UserDTO user { get; set; }
            public string message { get; set; }
        }

        private async void HandleLogin(object sender, EventArgs e)
        {

            try
            {
                var loginData = new
                {
                    username = usernameBox.Text,
                    password = passwordBox.Text
                };

                var response = await _client.PostAsJsonAsync("api/Users/login", loginData);
                response.EnsureSuccessStatusCode(); // Lanza excepción si hay error HTTP

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    MessageBox.Show($"¡Bienvenido/a {result.user.RoleName} {result.user.Name}!");

                    // ✅ Usar el gestor para mostrar el formulario de usuario
                    _appManager.ShowUserForm(result.user);

                    // ✅ Ocultar este login (NO cerrarlo)
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Credenciales inválidas");
                }
                
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error de red: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                MessageBox.Show("Tiempo de espera agotado");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        // ✅ Sobrescribir el cierre para evitar cerrar toda la app
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Solo ocultar, no cerrar realmente
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
            else
            {
                base.OnFormClosing(e);
            }
        }
    }
}