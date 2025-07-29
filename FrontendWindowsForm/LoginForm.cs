using System.Net.Http.Json;
using Models.Enums;
using DTOs;

namespace FrontendWindowsForm
{
    public partial class LoginForm : Form
    {
        private static readonly HttpClient client = new HttpClient();
        public LoginForm()
        {
            InitializeComponent();
            client.BaseAddress = new Uri("https://localhost:5001/");
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

                var response = await client.PostAsJsonAsync("api/Users/login", loginData);
                response.EnsureSuccessStatusCode(); // Lanza excepción si hay error HTTP

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    MessageBox.Show($"¡Bienvenido/a {result.user.RoleName} {result.user.Name}!");

                    this.Hide();

                    switch (result.user.RoleName)
                    {
                        case RoleType.Student:
                            new StudentForm(client, result.user).Show();
                            break;
                        case RoleType.Professor:
                            new ProfessorForm(client, result.user).Show();
                            break;
                        case RoleType.Administrator:
                            new AdministratorForm(client, result.user).Show();
                            break;
                        default:
                            MessageBox.Show("Rol no reconocido");
                            this.Show(); // Volver a mostrar login
                            return;
                    }

                    // ✅ Cerrar login cuando se abra el otro formulario
                    //this.Dispose();
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
    }
}