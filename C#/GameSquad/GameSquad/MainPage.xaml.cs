using GameSquad.View;

namespace GameSquad
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }

    }

}
