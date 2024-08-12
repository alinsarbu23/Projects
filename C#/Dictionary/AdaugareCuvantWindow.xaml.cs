using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System;
using System.Linq;
using System.Windows.Controls;

namespace Dictionar
{
    public partial class AdaugareCuvantWindow : Window
    {

        public AdaugareCuvantWindow()
        {
            InitializeComponent();
            IncarcaCategorii();
        }

        private void IncarcaCategorii()
        {
            string caleFisier = "cuvinte.txt";

            try
            {
                if (File.Exists(caleFisier))
                {
                    HashSet<string> categorii = new HashSet<string>();

                    string[] linii = File.ReadAllLines(caleFisier);

                    foreach (string linie in linii)
                    {
                        string[] cuvinte = linie.Split(' ');

                        if (cuvinte.Length >= 2)
                        {
                            categorii.Add(cuvinte[1]);
                        }
                    }

                    cbCategorie.ItemsSource = categorii;
                }
                else
                {
                    MessageBox.Show("Fișierul cuvinte.txt nu există.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A apărut o eroare la încărcarea categoriilor: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool VerificaCuvantExistenta(string cuvant)
        {
            try
            {
                string[] linii = File.ReadAllLines("cuvinte.txt");
                foreach (string linie in linii)
                {
                    // Verificăm dacă linia începe cu cuvântul căutat
                    if (linie.ToLower().StartsWith(cuvant.ToLower()))
                    {
                        return true;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Fișierul cu cuvinte nu a fost găsit.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la verificarea existenței cuvântului: " + ex.Message);
            }

            return false;
        }

        private void btnVerificare_Click(object sender, RoutedEventArgs e)
        {
            string nume = tbDenumireCuvant.Text.Trim();
            string categorie = tbDenumireCategorie.Text.Trim();
            string numePoza = Path.GetFileName(tbNumePoza.Text.Trim());
            string descriere = tbDescriere.Text.Trim();


            if (cbCategorie.Items.Contains(categorie))
            {
                MessageBox.Show("Categorie deja existentă. Introduceți o categorie nouă.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(nume) || string.IsNullOrEmpty(descriere))
            {
                MessageBox.Show("Toate câmpurile sunt obligatorii.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string cuvantExistenta = $"{nume.ToLower()} {categorie.ToLower()}";

            if (VerificaCuvantExistenta(cuvantExistenta))
            {
                MessageBox.Show("Cuvântul deja există în dicționar.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(categorie) && cbCategorie.SelectedItem != null)
            {
                categorie = cbCategorie.SelectedItem.ToString();
            }

            if (string.IsNullOrEmpty(numePoza))
            {
                numePoza = $"{nume}.png";
            }

            string caleImagine = $"{numePoza}";
            if (!File.Exists(caleImagine))
            {
                numePoza = "default.png";
            }

            string cuvantNou = $"{nume} {categorie} {numePoza} {descriere}";
            AdaugaCuvantInFisier(cuvantNou);

            // Mesaj de confirmare
            MessageBox.Show("Cuvântul a fost adăugat cu succes în dicționar.", "Confirmare", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void btnSelectarePoza_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fișiere imagine (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|Toate fișierele (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string numePoza = openFileDialog.FileName;
                tbNumePoza.Text = Path.GetFileName(numePoza);
                tbNumePozaSelectata.Text = tbNumePoza.Text;
            }
        }

        private void AdaugaCuvantInFisier(string cuvantNou)
        {
            // Calea către fișierul cu cuvinte
            string caleFisier = "cuvinte.txt";

            try
            {
                // Adăugăm cuvântul nou la fișier
                File.AppendAllLines(caleFisier, new[] { cuvantNou });

                // Reîncărcăm categoriile
                IncarcaCategorii();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A apărut o eroare la adăugarea cuvântului în fișier: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void SuprascrieCuvantExistenta(string nume, string categorie, string cuvantNou)
        {
            try
            {
                string[] linii = File.ReadAllLines("cuvinte.txt");
                for (int i = 0; i < linii.Length; i++)
                {
                    string linie = linii[i];

                    if (linie.ToLower().StartsWith($"{nume.ToLower()} {categorie.ToLower()}"))
                    {
                        // Suprascriem linia cu cuvântul nou
                        linii[i] = cuvantNou;

                        File.WriteAllLines("cuvinte.txt", linii);
                        return;
                    }
                }
                // Dacă nu am găsit cuvântul în fișier, adăugăm cuvântul nou la sfârșitul fișierului
                File.AppendAllLines("cuvinte.txt", new[] { cuvantNou });
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Fișierul cu cuvinte nu a fost găsit.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la suprascrierea cuvântului: " + ex.Message);
            }
        }


        public void AutocompletareCampuri(Cuvant cuvantSelectat)
        {
            try
            {
                tbDenumireCuvant.Text = cuvantSelectat.Nume;
                tbDenumireCategorie.Text = cuvantSelectat.Categorie;
                tbNumePoza.Text = cuvantSelectat.NumePoza;
                tbDescriere.Text = cuvantSelectat.Descriere;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la completarea câmpurilor de autocompletare: " + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEditare_Click(object sender, RoutedEventArgs e)
        {
            string nume = tbDenumireCuvant.Text.Trim();
            string categorie = tbDenumireCategorie.Text.Trim();
            string numePoza = Path.GetFileName(tbNumePoza.Text.Trim());
            string descriere = tbDescriere.Text.Trim();

            try
            {

                string[] linii = File.ReadAllLines("cuvinte.txt");

                bool gasitPotrivire = false;
                for (int i = 0; i < linii.Length; i++)
                {
                    string[] cuvinte = linii[i].Split(' ');

                    if (cuvinte.Length >= 4 && cuvinte[2].Equals(numePoza, StringComparison.OrdinalIgnoreCase))
                    {
                        string cuvantNou = $"{nume} {categorie} {cuvinte[2]} {descriere}";

                        linii[i] = cuvantNou;

                        gasitPotrivire = true;
                        break;
                    }
                }

                if (gasitPotrivire)
                {
                    File.WriteAllLines("cuvinte.txt", linii);
                    // Mesaj de confirmare
                    MessageBox.Show("Cuvântul a fost actualizat cu succes în dicționar.", "Confirmare", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Nu s-a găsit nicio potrivire pentru poza specificată.", "Avertizare", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la actualizarea cuvântului: " + ex.Message, "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}