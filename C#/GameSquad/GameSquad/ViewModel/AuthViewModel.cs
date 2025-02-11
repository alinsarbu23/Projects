using GameSquad.Model;
using GameSquad.Service;
using GameSquad.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }
    }
}

