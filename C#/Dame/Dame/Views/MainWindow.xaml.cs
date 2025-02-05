using System;
using System.Windows;
using System.Windows.Input;
using Dame.Business;
using Dame.Views;

namespace Dame
{
    public partial class MainWindow : Window
    {
        public ICommand NavigateToMenuCommand { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            NavigateToMenuCommand = new RelayCommand(NavigateToMenu);
            DataContext = this; 
        }

        private void NavigateToMenu(object obj)
        {
            MenuWindow menuWindow = new MenuWindow();
            menuWindow.Show();
            Close();
        }
    }
}
