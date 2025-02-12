using GameSquad.ViewModel;

namespace GameSquad.View
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage(RegisterViewModel registerViewModel)
        {
            InitializeComponent();
            BindingContext = registerViewModel;
        }
    }
}