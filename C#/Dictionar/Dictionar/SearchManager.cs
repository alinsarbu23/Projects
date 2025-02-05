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

    public partial class SearchManager
    {

        private List<string> allWords = new List<string>(); // Lista cu toate cuvintele
        private List<string> filteredWords = new List<string>(); // Lista cu cuvintele filtrate

        public SearchManager()
        {


        }

        public List<string> LoadCategories()
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

        public void LoadAllWords()
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

        public void LoadWordsByCategory(string category)
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


    }



}