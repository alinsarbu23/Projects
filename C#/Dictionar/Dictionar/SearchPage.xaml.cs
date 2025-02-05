using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Dictionar
{ 

    public partial class SearchPage : Window
    {
        private List<string> allWords = new List<string>(); // Lista cu toate cuvintele
        private List<string> filteredWords = new List<string>(); // Lista cu cuvintele filtrate

        public SearchPage()
        {
            InitializeComponent();

            List<string> categories = LoadCategories(); // Funcție pentru a încărca categoriile din baza de date sau fișier
            cbCategorie.ItemsSource = categories;

            LoadAllWords(); 
        }


        private List<string> LoadCategories()
        {
            List<string> categories = new List<string>();

            try
            {
                string[] linii = File.ReadAllLines("cuvinte.txt");
                foreach (string linie in linii)
                {
                    string[] cuvinte = linie.Split(' ');
                    if (cuvinte.Length >= 2)
                    {
                        string categorie = cuvinte[1]; // Se selectează al doilea cuvânt de pe fiecare linie
                        if (!categories.Contains(categorie))
                        {
                            categories.Add(categorie); // Se adaugă în lista de categorii doar dacă nu există deja
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Fișierul cu cuvinte nu a fost găsit.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea categoriilor: " + ex.Message);
            }

            return categories;
        }

        // Metoda pentru încărcarea tuturor primelor cuvinte de pe fiecare linie din dicționar
        private void LoadAllWords()
        {
            try
            {
                string[] linii = File.ReadAllLines("cuvinte.txt");
                foreach (string linie in linii)
                {
                    string[] cuvinte = linie.Split(' ');
                    if (cuvinte.Length >= 1)
                    {
                        string cuvant = cuvinte[0]; // Primul cuvânt de pe fiecare linie reprezintă cuvântul în sine
                        allWords.Add(cuvant.ToLower()); // Adăugăm cuvântul în lista de cuvinte, convertit la litere mici pentru a facilita compararea
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

        private void LoadWordsByCategory(string category)
        {
            try
            {
                allWords.Clear(); // Curățăm lista de cuvinte pentru a încărca doar cuvintele din categoria selectată
                string[] linii = File.ReadAllLines("cuvinte.txt");
                foreach (string linie in linii)
                {
                    string[] cuvinte = linie.Split(' ');
                    if (cuvinte.Length >= 2 && cuvinte[1] == category)
                    {
                        string cuvant = cuvinte[0]; // Primul cuvânt de pe fiecare linie reprezintă cuvântul în sine
                        allWords.Add(cuvant.ToLower()); // Adăugăm cuvântul în lista de cuvinte, convertit la litere mici pentru a facilita compararea
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


        private void TbCuvant_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = tbCuvant.Text.ToLower(); // Textul introdus în TextBox, convertit la litere mici pentru a facilita compararea

            // Obținem categoria selectată în combobox
            string selectedCategory = cbCategorie.SelectedItem as string;

            // Filtrăm cuvintele din dicționar
            if (!string.IsNullOrEmpty(searchText))
            {
                if (string.IsNullOrEmpty(selectedCategory))
                {
                    // Filtrăm cuvintele din dicționar care încep cu textul introdus
                    filteredWords = allWords.Where(word => word.StartsWith(searchText)).ToList();
                }
                else
                {
                    // Căutăm cuvintele din categoria selectată
                    LoadWordsByCategory(selectedCategory);
                    // Filtrăm cuvintele din lista de cuvinte încărcate după categorie
                    filteredWords = allWords.Where(word => word.StartsWith(searchText)).ToList();
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(selectedCategory))
                {
                    LoadWordsByCategory(selectedCategory);
                    filteredWords = allWords;
                }
                else
                {
                    filteredWords.Clear(); 
                }
            }

            lbCuvinte.ItemsSource = filteredWords;
        }

        private void ResetSearchFields()
        {
            tbCuvant.Clear(); 
            lbCuvinte.ItemsSource = null; 
            lbCuvinte.Visibility = Visibility.Collapsed; 
            lblSelectedWord.Content = "Cuvânt selectat:"; 
            lblCategory.Content = "Categorie:"; 
            lblDescription.Content = "Descriere:"; 
        }

        private void ShowWordDetails(string word)
        {
            try
            {
                string[] lines = File.ReadAllLines("cuvinte.txt");

                foreach (string line in lines)
                {
                    string[] words = line.Split(' ');
                    if (words.Length >= 3 && words[0].ToLower() == word.ToLower())
                    {
                        lblSelectedWord.Content = "Cuvânt selectat: " + words[0];
                        lblCategory.Content = "Categorie: " + words[1];

                        // Inițializăm descrierea cu cuvântul "Descriere:"
                        string description = "Descriere:";

                        for (int i = 3; i < words.Length; i++)
                        {
                            if (i == 3)
                                description += " " + words[i]; // Adăugăm primul cuvânt al descrierii
                            else
                                description += " " + words[i]; // Adăugăm restul cuvintelor descrierii
                        }
                        lblDescription.Content = description.Trim();

                        // Încărcăm imaginea asociată cuvântului
                        string imageName = words[2];
                        string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imageName);

                        if (File.Exists(imagePath))
                        {
                            BitmapImage image = new BitmapImage(new Uri(imagePath));
                            imgCuvant.Source = image;
                        }
                        else
                        {
                            string defaultImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "default.png");
                            BitmapImage defaultImage = new BitmapImage(new Uri(defaultImagePath));
                            imgCuvant.Source = defaultImage;
                        }

                        return;
                    }
                }

                MessageBox.Show("Cuvântul nu a fost găsit.");
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Fișierul cu cuvinte nu a fost găsit.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la afișarea detaliilor cuvântului: " + ex.Message);
            }
        }

        private void LbCuvinte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbCuvinte.SelectedItem != null)
            {
                string selectedWord = lbCuvinte.SelectedItem.ToString();

                tbCuvant.Text = selectedWord;

                lbCuvinte.Visibility = Visibility.Collapsed;

                tbCuvant.Clear();

                lbCuvinte.ItemsSource = null;

                ShowWordDetails(selectedWord);

                TbCuvant_TextChanged(sender, null);
            }
        }

        private void BtnSearchAnother_Click(object sender, RoutedEventArgs e)
        {
            
            ResetSearchFields();
            lbCuvinte.Visibility = Visibility.Visible;

        }

    }

}
