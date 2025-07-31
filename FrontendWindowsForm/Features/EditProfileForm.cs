using DTOs;


namespace FrontendWindowsForm.Features
{
    public partial class EditProfileForm : Form
    {
        private readonly HttpClient _client;
        private UserDTO _currentUser;
        public EditProfileForm(HttpClient client, UserDTO currentUser)
        {
            _client = client;
            _currentUser = currentUser;
            InitializeComponent();
        }
    }
}
