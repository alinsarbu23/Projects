using Dame.Business;
using Dame.Models;
using System.Windows;
using System.Windows.Controls;

namespace Dame.Views
{
    public partial class BoardWindow : Window
    {
        public Board board;
        public Grid ChessGrid { get; private set; }


        public BoardWindow(int boardSize, bool makePieceKing, bool startKing, bool allowmultiplejump)
        {
            InitializeComponent();
            board = new Board(Grid, boardSize, makePieceKing, startKing, allowmultiplejump);
            DataContext = board;
        }

    }
}