// MainWindow.xaml.cs
using System.Windows;
using System.Windows.Navigation;

namespace Dictionar
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.ShowDialog(); 
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchPage searchPage = new SearchPage();
            searchPage.ShowDialog();
        }

        private void btnEntertainment_Click(object sender, RoutedEventArgs e)
        {
            EntertainmentWindow entertainmentWindow = new EntertainmentWindow();
            entertainmentWindow.Show();
        }
    }
}
