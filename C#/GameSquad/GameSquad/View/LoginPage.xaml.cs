using GameSquad.Service;
using GameSquad.ViewModel;

namespace GameSquad.View;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new AuthViewModel(new AuthService(new UserService(new DatabaseService())));
    }

}