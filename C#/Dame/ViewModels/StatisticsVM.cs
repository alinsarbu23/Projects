using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Dame.ViewModels
{
    public class StatisticsVM : ViewModelBase
    {
        private string text;

        public string StatisticsText
        {
            get { return text; }
            set { Set(ref text, value); }
        }

        public RelayCommand LoadStatisticsCommand { get; private set; }

        public StatisticsVM()
        {
            DisplayStatistics();
        }

        public static void SaveStatistics(string castigator, int pieseMaximeRamase)
        {
            try
            {
                string cale_fisier = @"C:\Facultatea de Matematica si Informatica\Anul 2\Semestrul al doilea\MVP\Laborator\L6\Tema2\Dame\Resources\Statistics.txt";

                string[] lines = File.ReadAllLines(cale_fisier);

                if (castigator == "Jucătorul alb")
                {
                    lines[0] = (int.Parse(lines[0]) + 1).ToString();
                }

                else if (castigator == "Jucătorul roșu")
                {
                    lines[1] = (int.Parse(lines[1]) + 1).ToString();
                }

                int maxPiecesInFile = int.Parse(lines[2]);
                if (pieseMaximeRamase > maxPiecesInFile)
                {
                    lines[2] = pieseMaximeRamase.ToString();
                }

                File.WriteAllLines(cale_fisier, lines);

                MessageBox.Show("Statistica a fost salvată cu succes!", "Salvare reușită", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A apărut o eroare la salvarea statisticii: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void DisplayStatistics()
        {
            try
            {
                string cale_fisier = @"C:\Facultatea de Matematica si Informatica\Anul 2\Semestrul al doilea\MVP\Laborator\L6\Tema2\Dame\Resources\Statistics.txt";

                if (!File.Exists(cale_fisier))
                {
                    MessageBox.Show("Fisierul nu a fost gasit.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string[] lines = File.ReadAllLines(cale_fisier);

                if (lines.Length < 3)
                {
                    MessageBox.Show("Fisierul nu este complet.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int victoriiAlb = int.Parse(lines[0]);
                int victoriiRosu = int.Parse(lines[1]);
                int pieseMaximRamase = int.Parse(lines[2]);

                string statistici = $"Număr victorii alb: {victoriiAlb}\n";
                statistici += $"Număr victorii roșu: {victoriiRosu}\n";
                statistici += $"Număr maxim de piese rămase: {pieseMaximRamase}\n";

                MessageBox.Show(statistici, "Statistics", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A apărut o eroare la citirea fișierului: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
