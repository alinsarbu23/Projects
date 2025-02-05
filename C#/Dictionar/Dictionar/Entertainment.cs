using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Dictionar
{

    internal class Entertainment
    {
        private List<Cuvant> listaCuvinte = new List<Cuvant>();
        public List<Cuvant> selectedWords { get; private set; } = new List<Cuvant>();
        public List<string> hints { get; private set; } = new List<string>();

        public int currentIndex = 0;
        public int correctAnswers = 0;
        private Random random = new Random();

        public void StartGame()
        {
            IncarcaCuvinte();
            selectedWords = SelectRandomWords(5);

            GenerateHints();
        }

        private List<Cuvant> SelectRandomWords(int count)
        {
            List<Cuvant> randomWords = new List<Cuvant>();
            HashSet<int> usedIndices = new HashSet<int>();

            while (randomWords.Count < count && usedIndices.Count < listaCuvinte.Count)
            {
                int randomIndex = random.Next(0, listaCuvinte.Count);

                if (!usedIndices.Contains(randomIndex))
                {
                    randomWords.Add(listaCuvinte[randomIndex]);
                    usedIndices.Add(randomIndex);
                }
            }

            // Verificăm dacă am obținut suficiente cuvinte unice
            if (randomWords.Count < count)
            {
                MessageBox.Show("Nu există suficiente cuvinte unice disponibile în baza de date.");

            }

            return randomWords;
        }


        private void IncarcaCuvinte()
        {
            try
            {
                string[] linii = File.ReadAllLines("cuvinte.txt");
                foreach (string linie in linii)
                {
                    string[] informatii = linie.Split(' ');

                    if (informatii.Length >= 4)
                    {
                        string nume = informatii[0];
                        string categorie = informatii[1];
                        string numePoza = informatii[2];

                        string descriere = string.Join(" ", informatii, 3, informatii.Length - 3);

                        string caleImagine = Path.Combine(Directory.GetCurrentDirectory(), numePoza);

                        if (!File.Exists(caleImagine))
                        {
                            caleImagine = Path.Combine(Directory.GetCurrentDirectory(), "default.png");
                        }

                        BitmapImage imagine = new BitmapImage(new Uri(caleImagine, UriKind.RelativeOrAbsolute));

                        Cuvant cuvant = new Cuvant
                        {
                            Nume = nume,
                            Categorie = categorie,
                            NumePoza = numePoza,
                            Descriere = descriere,
                            Imagine = imagine
                        };
                        listaCuvinte.Add(cuvant);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Fișierul cu cuvinte nu a fost găsit.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea cuvintelor: " + ex.Message);
            }
        }

        private void GenerateHints()
        {
            foreach (Cuvant cuvant in selectedWords)
            {
                int hintType = random.Next(2);

                if (hintType == 0 || cuvant.NumePoza == "default.png")
                {
                    hints.Add(cuvant.Descriere);
                }
                else
                {
                    hints.Add(cuvant.NumePoza);
                }
            }
        }

        public void CheckAnswer(string userAnswer)
        {
            if (currentIndex >= 0 && currentIndex < selectedWords.Count)
            {
                string correctAnswer = selectedWords[currentIndex].Nume.ToLower();
                string userInputLower = userAnswer.ToLower();

                if (userInputLower.Equals(correctAnswer))
                {
                    MessageBox.Show("Correct!");
                    correctAnswers++;
                }
                else if (userInputLower.Equals(selectedWords[currentIndex].Nume.ToLower()))
                {
                    MessageBox.Show("Case insensitive match! The correct answer has different case.");
                }
                else
                {
                    MessageBox.Show($"Incorrect! Răspunsul corect este: {correctAnswer}");
                }
            }
            else
            {
                MessageBox.Show("Error: Index out of range!");
            }
        }

        public void ResetGame()
        {
            selectedWords.Clear();
            hints.Clear();
            currentIndex = 0;
            correctAnswers = 0;
        }

    }
}
