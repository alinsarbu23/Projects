using System;
using System.Reflection;
using System.Windows;

namespace Dictionar
{

    public partial class AdministratorWindow : Window
    {
        private AfisareCuvinteWindow afisareCuvinteWindow;
        private AdaugareCuvantWindow adaugareCuvantWindow;

        public AdministratorWindow()
        {
            InitializeComponent();

            afisareCuvinteWindow = new AfisareCuvinteWindow();
            adaugareCuvantWindow = new AdaugareCuvantWindow();
        }

        private void btnAdauga_Click(object sender, RoutedEventArgs e)
        {
            AdaugareCuvantWindow adaugareCuvantWindow = new AdaugareCuvantWindow();
            adaugareCuvantWindow.Show();
        }

        private void btnAfisare_Click(object sender, RoutedEventArgs e)
        {
            afisareCuvinteWindow = new AfisareCuvinteWindow();
            afisareCuvinteWindow.Show();
        }

        private void btnEditare_Click(object sender, RoutedEventArgs e)
        {
            int indexSelectat = afisareCuvinteWindow.listViewDetalii.SelectedIndex;

            if (indexSelectat >= 0)
            {
                Cuvant cuvantSelectat = afisareCuvinteWindow.GetCuvantByIndex(indexSelectat);

                if (adaugareCuvantWindow == null || !adaugareCuvantWindow.IsVisible)
                {
                    adaugareCuvantWindow = new AdaugareCuvantWindow();
                }

                adaugareCuvantWindow.AutocompletareCampuri(cuvantSelectat);

                // Verificăm dacă fereastra este deja deschisă și dacă nu, o deschidem
                if (!adaugareCuvantWindow.IsVisible)
                {
                    adaugareCuvantWindow.Show();
                }
                else
                {
                    adaugareCuvantWindow.Focus(); // este în prim-plan
                }
            }
            else
            {
                MessageBox.Show("Vă rugăm să selectați un cuvânt pentru a-l edita.", "Avertizare", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnStergere_Click(object sender, RoutedEventArgs e)
        {


            if (afisareCuvinteWindow.listViewDetalii.SelectedItem != null)
            {



                MessageBoxResult result = MessageBox.Show("Sigur doriți să ștergeți acest cuvânt?", "Confirmare ștergere", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    int index = afisareCuvinteWindow.listViewDetalii.SelectedIndex;

                    afisareCuvinteWindow.StergeCuvant(index);
                }
            }
            else
            {
                MessageBox.Show("Vă rugăm să selectați un cuvânt pentru a-l șterge.", "Avertizare", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}