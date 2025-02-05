using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.IO;
using System.Windows;

namespace Dame.ViewModels
{
    public class AboutInfoViewModel : ViewModelBase
    {
        private string numeStudent;
        private string adresaEmail;
        private string grupa;
        private string descriere;

        public string StudentName
        {
            get { return numeStudent; }
            set { Set(ref numeStudent, value); }
        }

        public string EmailAddress
        {
            get { return adresaEmail; }
            set { Set(ref adresaEmail, value); }
        }

        public string Group
        {
            get { return grupa; }
            set { Set(ref grupa, value); }
        }

        public string GameDescription
        {
            get { return descriere; }
            set { Set(ref descriere, value); }
        }

        public RelayCommand LoadAboutInfoCommand { get; private set; }

        public AboutInfoViewModel()
        {
            LoadAboutInfoCommand = new RelayCommand(LoadAboutInfo);
        }

        public void LoadAboutInfo()
        {
            try
            {
                string filePath = @"C:\Facultatea de Matematica si Informatica\Anul 2\Semestrul al doilea\MVP\Laborator\L6\Tema2\Dame\Resources\AboutInfo.txt";

                string[] lines = File.ReadAllLines(filePath);

                if (lines.Length >= 4)
                {
                    StudentName = lines[0];
                    EmailAddress = lines[1];
                    Group = lines[2];
                    GameDescription = lines[3];
                }
                else
                {
                    MessageBox.Show("Fișierul AboutInfo.txt nu conține suficiente informații.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A apărut o eroare la citirea fișierului: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
