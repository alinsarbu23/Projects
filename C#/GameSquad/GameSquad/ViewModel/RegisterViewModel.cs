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
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _authService;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Username { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }

        public ICommand RegisterCommand { get; }
        public ICommand NavigateToHomeCommand { get; }

        public RegisterViewModel(AuthService authService)
        {
            _authService = authService;
            RegisterCommand = new Command(async () => await Register());
            NavigateToHomeCommand = new Command(async () => await NavigateToHome());
        }

        private async Task Register()
        {
            User user = new User { Id = Guid.NewGuid().ToString(), Username = Username, Password = Password };
            bool success = await _authService.RegisterUserAsync(user);
            Message = success ? "Înregistrare reușită!" : "Username deja folosit!";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));

            if (success)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new HomePage());
            }
        }

        private async Task NavigateToHome()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new HomePage());
        }
    }
}

