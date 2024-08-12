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

    public partial class AfisareCuvinteWindow : Window
    {
        private List<Cuvant> listaCuvinte = new List<Cuvant>(); // Inițializare lista
        public int indexPaginaCurenta = 0;
        public int numarCuvintePePagina = 5;
        private List<string> cuvinte;


        public AfisareCuvinteWindow()
        {
            InitializeComponent();
            IncarcaCuvinte(); // Încărcați cuvintele în lista
            AfiseazaPagina(indexPaginaCurenta);
            cuvinte = new List<string>();
        }

        public void SetCuvinte(List<string> cuvinte)
        {
            this.cuvinte = cuvinte;
        }

        public int GetPageIndex()
        {
            return indexPaginaCurenta; 
        }


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

        public void AfiseazaPagina(int indexPagina)
        {
            int startIndex = indexPagina * numarCuvintePePagina;
            int endIndex = startIndex + numarCuvintePePagina;

            if (endIndex > listaCuvinte.Count)
            {
                endIndex = listaCuvinte.Count;
            }

            listViewDetalii.Items.Clear();

            for (int i = startIndex; i < endIndex; i++)
            {
                var cuvant = listaCuvinte[i];

                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;

                // Adăugare imagine
                Image image = new Image();
                image.Source = cuvant.Imagine;
                image.Width = 100;
                image.Height = 100;
                stackPanel.Children.Add(image);

                TextBlock textBlock = new TextBlock();
                textBlock.Text = $"Nume: {cuvant.Nume}\nCategorie: {cuvant.Categorie}\nDescriere: {cuvant.Descriere}";
                textBlock.Margin = new Thickness(10, 0, 0, 0);
                stackPanel.Children.Add(textBlock);

                ListViewItem item = new ListViewItem();
                item.Content = stackPanel;
                listViewDetalii.Items.Add(item);
            }
        }

        public Cuvant GetCuvantByIndex(int index)
        {
            if (index >= 0 && index < listaCuvinte.Count)
            {
                return listaCuvinte[index];
            }
            return new Cuvant();
        }


        public void StergeCuvant(int index)
        {
            try
            {
                if (index >= 0 && index < listViewDetalii.Items.Count)
                {
                    var item = (ListViewItem)listViewDetalii.Items[index];
                    var cuvantSters = listaCuvinte[indexPaginaCurenta * numarCuvintePePagina + index];

                    // Ștergem cuvântul din lista de cuvinte
                    listaCuvinte.Remove(cuvantSters);

                    // Ștergem cuvântul din fișierul cuvinte.txt
                    List<string> linii = File.ReadAllLines("cuvinte.txt").ToList();
                    string linieCautata = $"{cuvantSters.Nume} {cuvantSters.Categorie} {cuvantSters.NumePoza} {cuvantSters.Descriere}";
                    linii.Remove(linieCautata);
                    File.WriteAllLines("cuvinte.txt", linii);

                    // Reafișăm pagina pentru a reflecta modificările
                    AfiseazaPagina(indexPaginaCurenta);
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

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if ((indexPaginaCurenta + 1) * numarCuvintePePagina < listaCuvinte.Count)
            {
                indexPaginaCurenta++;
                AfiseazaPagina(indexPaginaCurenta);
            }
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (indexPaginaCurenta > 0)
            {
                indexPaginaCurenta--;
                AfiseazaPagina(indexPaginaCurenta);
            }
        }



    }
}
