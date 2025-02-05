using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Dictionar
{
    public partial class EntertainmentWindow : Window
    {
        private Entertainment entertainment;

        public EntertainmentWindow()
        {
            InitializeComponent();
            entertainment = new Entertainment();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (entertainment.currentIndex < entertainment.selectedWords.Count - 1)
            {
                entertainment.currentIndex++; // Trecem la următorul cuvânt și indiciu
                DisplayCurrentHint(); // Afișăm indiciul pentru cuvântul următor
                UpdateWordNumber(); // Actualizăm numărul cuvântului
                Optiune.Clear(); // Ștergem textul din caseta de text pentru următorul răspuns
            }
            else
            {
                // Jocul s-a încheiat, afișăm un mesaj corespunzător
                MessageBox.Show($"Jocul s-a încheiat! Ai obtinut {txtScore.Text} puncte din 5 posibile! Felicitări!");
                // Resetăm jocul pentru a putea fi jucat din nou
                entertainment.ResetGame();
                txtScore.Text = "0"; // Resetăm scorul la 0
                txtWordNumber.Text = "0"; // Resetăm numărul cuvântului la 0
                txtHint.Text = ""; // Ștergem indiciile
                pic1.Source = null; // Ștergem sursa imaginii
            }
        }


        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            entertainment.StartGame();
            txtWordNumber.Text = "1"; // Inițializăm numărul cuvântului la 1
            txtScore.Text = "0"; // Inițializăm scorul la 0
            DisplayCurrentHint(); // Afișăm indiciul pentru primul cuvânt
        }

        private void btnCheckAnswer_Click(object sender, RoutedEventArgs e)
        {
            string userAnswer = Optiune.Text.Trim();
            entertainment.CheckAnswer(userAnswer);

            txtScore.Text = entertainment.correctAnswers.ToString();

            // Trecem la următorul cuvânt și indiciu
            if (entertainment.currentIndex < entertainment.selectedWords.Count - 1)
            {
                entertainment.currentIndex++;
                DisplayCurrentHint(); // Afișăm indiciul pentru cuvântul următor
                UpdateWordNumber(); // Actualizăm numărul cuvântului
                Optiune.Clear(); // Ștergem textul din caseta de text pentru următorul răspuns
            }
            else
            {
                // Jocul s-a încheiat, afișăm un mesaj corespunzător
                MessageBox.Show($"Jocul s-a încheiat. Ai obținut {txtScore.Text} puncte din 5 posibile ! Felicitări !");
                // Resetăm jocul pentru a putea fi jucat din nou
                entertainment.ResetGame();
                txtScore.Text = "0"; // Resetăm scorul la 0
                txtWordNumber.Text = "0"; // Resetăm numărul cuvântului la 0
                txtHint.Text = ""; // Ștergem indiciile
                pic1.Source = null; // Ștergem sursa imaginii
            }
        }





        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            Optiune.Clear();
            entertainment.ResetGame();
        }

        private void Optiune_TextChanged(object sender, TextChangedEventArgs e)
        {
            string userWord = Optiune.Text.Trim();
            if (!string.IsNullOrWhiteSpace(userWord))
            {
                entertainment.CheckAnswer(userWord);
            }
        }

        private void DisplayCurrentHint()
        {
            if (entertainment.currentIndex >= 0 && entertainment.currentIndex < entertainment.selectedWords.Count)
            {
                Cuvant currentWord = entertainment.selectedWords[entertainment.currentIndex];
                string currentHint = entertainment.hints[entertainment.currentIndex];

                if (currentHint == currentWord.NumePoza)
                {
                    string imageName = currentWord.NumePoza;
                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imageName);

                    if (File.Exists(imagePath))
                    {
                        BitmapImage image = new BitmapImage(new Uri(imagePath));
                        pic1.Source = image;
                    }

                    txtHint.Text = "";

                }

                else if (currentHint == currentWord.Descriere)
                {
                    
                    txtHint.Text = currentHint;
                    pic1.Source = null; 
                }
            }
            else
            {
                txtHint.Text = "No hint available for current word.";
                pic1.Source = null; // Asigurăm că nu se afișează nicio imagine dacă nu există indiciu
            }
        }


        private void UpdateWordNumber()
        {
            txtWordNumber.Text = (entertainment.currentIndex + 1).ToString();
        }

    }
}
