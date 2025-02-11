using GameSquad.Service;
using GameSquad.View;
using GameSquad.ViewModel;

namespace GameSquad
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage();
        }
    }
}

