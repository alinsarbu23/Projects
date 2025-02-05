using System.Windows;

namespace Dictionar
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            UserAccountManager userAccountManager = new UserAccountManager();

            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (userAccountManager.ValidateUser(username, password))
            {
                MessageBox.Show("Autentificare reușită! Poți accesa acum modulul administrativ.", "Succes");

                AdministratorWindow adminWindow = new AdministratorWindow();
                adminWindow.Show();

                this.Close(); // Închide fereastra de autentificare
            }
            else
            {
                MessageBox.Show("Autentificare eșuată! Numele de utilizator sau parola incorecte.", "Eroare");
            }
        }


    }
}
