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

    public struct Cuvant
    {
        public string Nume { get; set; }
        public string Categorie { get; set; }
        public string NumePoza { get; set; }
        public string Descriere { get; set; }
        public BitmapImage Imagine { get; set; } // Imaginea asociată cuvântului
    }


    public class CuvinteManager
    {
        private List<Cuvant> listaCuvinte = new List<Cuvant>();
        private int indexPaginaCurenta = 0;
        private int numarCuvintePePagina = 5;
        private List<string> cuvinte;


        public string GetSelectedWordByIndex(int index)
        {
            if (index >= 0 && index < cuvinte.Count)
            {
                return cuvinte[index];
            }
            return null;
        }

        public void IncarcaCuvinte()
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

                        string descriere = string.Join(" ", informatii.Skip(3));

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

        public void StergeCuvant(int index, string fisier)
        {
            try
            {
                if (index >= 0 && index < listaCuvinte.Count)
                {
                    var cuvantSters = listaCuvinte[index];
                    listaCuvinte.RemoveAt(index);

                    string[] linii = listaCuvinte.Select(cuvant => $"{cuvant.Nume} {cuvant.Categorie} {cuvant.NumePoza} {cuvant.Descriere}").ToArray();
                    File.WriteAllLines(fisier, linii);
                }
                else
                {
                    MessageBox.Show("Indexul cuvântului este invalid.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la ștergerea cuvântului: " + ex.Message);
            }
        }

        public Cuvant GetCuvantByIndex(int index)
        {
            if (index >= 0 && index < listaCuvinte.Count)
            {
                return listaCuvinte[index];
            }
            return new Cuvant(); // În cazul în care indexul este în afara intervalului
        }

        public void NextPage()
        {
            if ((indexPaginaCurenta + 1) * numarCuvintePePagina < listaCuvinte.Count)
            {
                indexPaginaCurenta++;
            }
        }

        public void PreviousPage()
        {
            if (indexPaginaCurenta > 0)
            {
                indexPaginaCurenta--;
            }
        }
    }
}
