using GameSquad.Model;
using GameSquad.Service;
using GameSquad.View;
using GameSquad.ViewModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GameSquad.ViewModel
{
    public class AuthViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Username { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }

        public ICommand LoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }

        public AuthViewModel(AuthService authService)
        {
            _authService = authService;
            LoginCommand = new Command(async () => await Login());
            NavigateToRegisterCommand = new Command(async () => await NavigateToRegister());
        }

        private async Task Login()
        {
            bool success = await _authService.Login(Username, Password);
            Message = success ? "Login reușit!" : "Autentificare eșuată";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));

            if (success)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new HomePage());
            }
        }

        private async Task NavigateToRegister()
        {
            var registerViewModel = new RegisterViewModel(new AuthService(new UserService(new DatabaseService())));
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage(registerViewModel));
        }
    }
}
