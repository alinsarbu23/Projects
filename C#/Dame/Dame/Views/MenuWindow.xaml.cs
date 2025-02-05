using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Dame.Models;
using Dame.ViewModels;
using Microsoft.Win32;

namespace Dame.Views
{
    public partial class MenuWindow : Window
    {

        private AboutInfoViewModel aboutInfoViewModel;
        private BoardWindow boardWindow;

        private int boardSize;
        private bool makePieceKing;
        private bool startKing;
        private bool allowmultiplejump;


        public MenuWindow()
        {
            InitializeComponent();
            aboutInfoViewModel = new AboutInfoViewModel();
            DataContext = aboutInfoViewModel;
        }

        private void Button_NewGame_Click(object sender, RoutedEventArgs e)
        {
            if(boardSize == null)
            {
                boardSize = 8;
            }

            if (makePieceKing == null)
            {
                makePieceKing = false;
            }

            if (startKing == null)
            {
                startKing = false;
            }

            if (allowmultiplejump == null)
            {
                allowmultiplejump = false;
            }

            boardWindow = new BoardWindow(boardSize, makePieceKing, startKing, allowmultiplejump);
            boardWindow.Show();
        }


        private void SaveGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fereastra_salvare = new SaveFileDialog();
                fereastra_salvare.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

                if (fereastra_salvare.ShowDialog() == true)
                {

                    string cale_fisier = fereastra_salvare.FileName;

                    using (StreamWriter writer = new StreamWriter(cale_fisier))
                    {
                        writer.WriteLine(boardSize);
                        writer.WriteLine(makePieceKing);
                        writer.WriteLine(startKing);
                        writer.WriteLine(allowmultiplejump);
                        writer.WriteLine(boardWindow.board.CurrentPlayer);


                        writer.WriteLine(boardWindow.board.pieseRosu);
                        writer.WriteLine(boardWindow.board.pieseAlb);

                        foreach (var square in boardWindow.board.GameBoard)
                        {
                            if (square.Piece != null)
                            {
                                writer.WriteLine($"{square.Row} {square.Column} {square.Piece.Color} {square.Piece.Type} {square.Piece.IsKing}");
                            }
                        }
                    }

                    MessageBox.Show("Jocul a fost salvat cu succes!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la salvarea jocului: {ex.Message}");
            }
        }


        private void LoadGame_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var windowGame = new OpenFileDialog();
                windowGame.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

                if (windowGame.ShowDialog() == true)
                {
                    string cale_fisier = windowGame.FileName;

                    if (boardWindow != null)
                    {
                        boardWindow.Close();
                        boardWindow = null;
                    }

                    using (StreamReader reader = new StreamReader(cale_fisier))
                    {
                        boardSize = int.Parse(reader.ReadLine());
                        makePieceKing = bool.Parse(reader.ReadLine());
                        startKing = bool.Parse(reader.ReadLine());
                        allowmultiplejump = bool.Parse(reader.ReadLine());

                        boardWindow = new BoardWindow(boardSize, makePieceKing, startKing, allowmultiplejump);

                        foreach (var square in boardWindow.board.GameBoard)
                        {
                            square.Piece = null;
                        }

                        boardWindow.board.CurrentPlayer = (PieceColor)Enum.Parse(typeof(PieceColor), reader.ReadLine());
                        boardWindow.board.pieseRosu = int.Parse(reader.ReadLine());
                        boardWindow.board.pieseAlb = int.Parse(reader.ReadLine());

                        while (!reader.EndOfStream)
                        {
                            string[] linie = reader.ReadLine().Split(' ');
                            int row = int.Parse(linie[0]);
                            int col = int.Parse(linie[1]);
                            PieceColor color = (PieceColor)Enum.Parse(typeof(PieceColor), linie[2]);
                            PieceType type = (PieceType)Enum.Parse(typeof(PieceType), linie[3]);
                            bool isKing = bool.Parse(linie[4]);

                            var square = boardWindow.board.GameBoard.FirstOrDefault(s => s.Row == row && s.Column == col);
                            if (square != null)
                            {
                                square.Piece = new Piece("Piece", color, type, isKing);
                            }
                        }

                        boardWindow.board.UpdateBoardView();
                    }

                    boardWindow.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la încărcarea jocului: {ex.Message}");
            }
        }


        private void Button_Statistics_Click(object sender, RoutedEventArgs e)
        {
            StatisticsVM statisticsVM = new StatisticsVM();
        }


        private void About_Click(object sender, RoutedEventArgs e)
        {
            aboutPanel.Visibility = Visibility.Visible;
            aboutInfoViewModel.LoadAboutInfo();
        }


        private void MenuItem_BoardSize_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            string header = menuItem.Header.ToString();
            boardSize = int.Parse(header.Split('x')[0]);
        }


        private void MakePieceKing_Checked(object sender, RoutedEventArgs e)
        {
            makePieceKing = true;
        }


        private void MakePieceKing_Unchecked(object sender, RoutedEventArgs e)
        {
            makePieceKing = false;
        }


        private void StartKing_Checked(object sender, RoutedEventArgs e)
        {
            startKing = true;
        }


        private void StartKing_Unchecked(object sender, RoutedEventArgs e)
        {
            startKing = false;
        }


        private void Allow_Multiple_Jump_Checked(object sender, RoutedEventArgs e)
        {
            allowmultiplejump = true;
        }

        private void Allow_Multiple_Jump_Unchecked(object sender, RoutedEventArgs e)
        {
            allowmultiplejump = false;
        }


    }
}