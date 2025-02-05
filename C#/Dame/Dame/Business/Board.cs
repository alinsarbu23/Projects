using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Dame.Models;
using Dame.ViewModels;

namespace Dame.Business
{
    public class Board : INotifyPropertyChanged
    {
        private const int ButtonSize = 60;

        //Optiuni de joc
        private int BoardSize;
        private bool makePieceKing;
        private bool startKing;
        private bool allowmultiplejump;

        //Verificare saritura multipla
        private bool hasJumped;

        private GameSquare pozitieSelectata;
        private Piece piesaSelectata;
        private bool isPieceSelected = false;

        //Numarare piese
        public int pieseRosu { get; set; }
        public int pieseAlb { get; set; }


        //Schimbare jucator front end
        public PieceColor currentPlayer;
        public event PropertyChangedEventHandler PropertyChanged;


        //Tabla de joc
        public ObservableCollection<GameSquare> GameBoard { get; private set; }
        public Grid ChessGrid { get; private set; }


        public Board(Grid chessGrid, int boardSize, bool makePieceKing, bool startKing, bool allowmultiplejump)
        {
            this.BoardSize = boardSize;
            this.makePieceKing = makePieceKing;
            this.startKing = startKing;
            this.allowmultiplejump = allowmultiplejump;

            ChessGrid = chessGrid;
            GameBoard = new ObservableCollection<GameSquare>();

            InitializeGameBoard();
            GenerateBoard();
            InitializePiecesCount();
        }



        private void InitializePiecesCount()
        {
            pieseRosu = GameBoard.Count(square => square.Piece != null && square.Piece.Color == PieceColor.Red);
            pieseAlb = GameBoard.Count(square => square.Piece != null && square.Piece.Color == PieceColor.White);
        }


        public void InitializeGameBoard()
        {
            if (BoardSize == 0)
            {
                BoardSize = 8;
            }

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    SquareShade nuanta = (row + col) % 2 == 0 ? SquareShade.Dark : SquareShade.Light;

                    Piece piece = null;
                    int pieseMaxim = BoardSize / 2 - 1;

                    if (row < pieseMaxim || row >= BoardSize - pieseMaxim) //Saptiul tablei la mijloc
                    {
                        if (nuanta == SquareShade.Dark && startKing == false)
                        {
                            PieceColor pieceColor = row < pieseMaxim ? PieceColor.White : PieceColor.Red;
                            PieceType pieceType = PieceType.Regular;
                            piece = new Piece("Piece", pieceColor, pieceType, false);
                        }

                        else if (nuanta == SquareShade.Dark && startKing == true)
                        {
                            PieceColor pieceColor = row < pieseMaxim ? PieceColor.White : PieceColor.Red;
                            PieceType pieceType = PieceType.King;
                            piece = new Piece("Piece", pieceColor, pieceType, true);
                        }
                    }

                    GameSquare square = new GameSquare(row, col, nuanta, piece);
                    GameBoard.Add(square);
                }
            }

            CurrentPlayer = PieceColor.Red;
        }


        private void GenerateBoard()
        {

            for (int row = 0; row < BoardSize; row++)
            {
                ChessGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(ButtonSize) });
            }

            for (int col = 0; col < BoardSize; col++)
            {
                ChessGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(ButtonSize) });
            }

            SolidColorBrush lightColor = new SolidColorBrush(Color.FromRgb(238, 220, 151));
            SolidColorBrush darkColor = new SolidColorBrush(Color.FromRgb(36, 26, 15));
            SolidColorBrush selectedColor = new SolidColorBrush(Color.FromRgb(255, 255, 0)); // culoarea piesa selectata (galben)
            SolidColorBrush hintColor = new SolidColorBrush(Color.FromRgb(0, 255, 0)); // culoarea pentru casetele valide

            foreach (var square in GameBoard)
            {
                Button button = new Button();
                if ((square.Row + square.Column) % 2 == 0)
                {
                    button.Background = lightColor;
                }
                else
                {
                    button.Background = darkColor;
                }

                if (square.Piece != null)
                {
                    Image pieceImage = new Image();
                    pieceImage.Source = new BitmapImage(new Uri(square.Piece.GetPieceImagePath()));
                    button.Content = pieceImage;
                }

                button.Click += (sender, e) => OnSquareClicked(square);

                ChessGrid.Children.Add(button);
                Grid.SetRow(button, square.Row);
                Grid.SetColumn(button, square.Column);
            }
        }


        private void ClearHintPositions()
        {
            foreach (var square in GameBoard)
            {
                if (square.IsHint)
                {
                    square.IsHint = false;
                }
            }
        }


        private void MarkValidMovePositions(GameSquare selectedSquare)
        {
            foreach (var square in GameBoard)
            {
                if (IsValidMove(selectedSquare, square) && square.Piece == null)
                {
                    square.IsHint = true;
                }
            }
        }


        private bool IsValidMove(GameSquare sourceSquare, GameSquare targetSquare)
        {
            if (sourceSquare.Piece != null)
            {
                if (sourceSquare.Piece.Type == PieceType.Regular)
                {
                    if (sourceSquare.Piece.Color == PieceColor.Red)
                    {
                        // Verificăm mișcarea piesei roșii în sus
                        if (targetSquare.Row == sourceSquare.Row - 1 && Math.Abs(targetSquare.Column - sourceSquare.Column) == 1)
                        {
                            return true;
                        }
                        // Verificăm săritura piesei roșii în sus
                        else if (targetSquare.Row == sourceSquare.Row - 2 && Math.Abs(targetSquare.Column - sourceSquare.Column) == 2)
                        {
                            int jumpedRow = (sourceSquare.Row + targetSquare.Row) / 2;
                            int jumpedCol = (sourceSquare.Column + targetSquare.Column) / 2;
                            GameSquare jumpedSquare = GameBoard.FirstOrDefault(s => s.Row == jumpedRow && s.Column == jumpedCol);
                            return jumpedSquare != null && jumpedSquare.Piece != null && jumpedSquare.Piece.Color != sourceSquare.Piece.Color;
                        }
                    }


                    else if (sourceSquare.Piece.Color == PieceColor.White)
                    {
                        // Verificăm mișcarea piesei albe în jos
                        if (targetSquare.Row == sourceSquare.Row + 1 && Math.Abs(targetSquare.Column - sourceSquare.Column) == 1)
                        {
                            return true;
                        }
                        // Verificăm săritura piesei albe în jos
                        else if (targetSquare.Row == sourceSquare.Row + 2 && Math.Abs(targetSquare.Column - sourceSquare.Column) == 2)
                        {
                            int jumpedRow = (sourceSquare.Row + targetSquare.Row) / 2;
                            int jumpedCol = (sourceSquare.Column + targetSquare.Column) / 2;
                            GameSquare jumpedSquare = GameBoard.FirstOrDefault(s => s.Row == jumpedRow && s.Column == jumpedCol);
                            return jumpedSquare != null && jumpedSquare.Piece != null && jumpedSquare.Piece.Color != sourceSquare.Piece.Color;
                        }
                    }

                }

                else if (sourceSquare.Piece.Type == PieceType.King)
                {

                    if ((Math.Abs(targetSquare.Row - sourceSquare.Row) == 1 && Math.Abs(targetSquare.Column - sourceSquare.Column) == 1) || // miscare simpla
                        (Math.Abs(targetSquare.Row - sourceSquare.Row) == 2 && Math.Abs(targetSquare.Column - sourceSquare.Column) == 2)) // saritura
                    {
                        if (Math.Abs(targetSquare.Row - sourceSquare.Row) == 2 && Math.Abs(targetSquare.Column - sourceSquare.Column) == 2)
                        {
                            int jumpedRow = (sourceSquare.Row + targetSquare.Row) / 2;
                            int jumpedCol = (sourceSquare.Column + targetSquare.Column) / 2;
                            GameSquare jumpedSquare = GameBoard.FirstOrDefault(s => s.Row == jumpedRow && s.Column == jumpedCol);
                            return jumpedSquare != null && jumpedSquare.Piece != null && jumpedSquare.Piece.Color != sourceSquare.Piece.Color;
                        }
                        return true;
                    }
                }
            }

            return false;
        }


        private void OnSquareClicked(GameSquare clickedSquare)
        {
            if (!isPieceSelected)
            {
                if (clickedSquare.Piece != null && clickedSquare.Piece.Color == CurrentPlayer)
                {
                    pozitieSelectata = clickedSquare;
                    piesaSelectata = pozitieSelectata.Piece;
                    isPieceSelected = true;
                    MarkValidMovePositions(pozitieSelectata);
                }
            }
            else
            {
                // Dacă o piesă a fost deja selectată, verificăm dacă caseta apăsată este una dintre mutările valide
                if (clickedSquare.IsHint)
                {

                    PerformMove(pozitieSelectata, clickedSquare);

                    if (allowmultiplejump && CanPieceJumpAgain(clickedSquare) && hasJumped)
                    {

                        pozitieSelectata = clickedSquare;
                        ClearHintPositions();
                        MarkValidMovePositions(pozitieSelectata);
                        return;
                    }

                    isPieceSelected = false;
                    piesaSelectata = null;
                    hasJumped = false;
                    ClearHintPositions();

                    CurrentPlayer = CurrentPlayer == PieceColor.Red ? PieceColor.White : PieceColor.Red;
                }

                else // nu este indiciu => nu este o pozitie valida
                {

                    isPieceSelected = false;
                    piesaSelectata = null;

                    ClearHintPositions();
                }
            }

            UpdateBoardView();
        }


        private bool CanPieceJumpAgain(GameSquare square)
        {
            foreach (var targetSquare in GameBoard)
            {
                if (IsValidMove(square, targetSquare) && targetSquare.Piece == null && Math.Abs(targetSquare.Row - square.Row) == 2)
                {
                    int jumpedRow = (square.Row + targetSquare.Row) / 2;
                    int jumpedCol = (square.Column + targetSquare.Column) / 2;
                    GameSquare jumpedSquare = GameBoard.FirstOrDefault(s => s.Row == jumpedRow && s.Column == jumpedCol);
                    if (jumpedSquare != null && jumpedSquare.Piece != null && jumpedSquare.Piece.Color != square.Piece.Color)
                    {
                        return true;
                    }
                }
            }
            return false;
        }



        private void PerformMove(GameSquare sourceSquare, GameSquare targetSquare)
        {
            if (targetSquare.Piece == null || targetSquare.Piece.Color != sourceSquare.Piece.Color)
            {
                targetSquare.Piece = sourceSquare.Piece; //Inlocuire sursa clisk inainte de atac
                sourceSquare.Piece = null;

                if (Math.Abs(targetSquare.Row - sourceSquare.Row) == 2)
                {
                    int jumpedRow = (sourceSquare.Row + targetSquare.Row) / 2;
                    int jumpedCol = (sourceSquare.Column + targetSquare.Column) / 2;
                    GameSquare jumpedSquare = GameBoard.FirstOrDefault(s => s.Row == jumpedRow && s.Column == jumpedCol); //inlocuire piesa

                    if (jumpedSquare != null && makePieceKing == true) //modul makePieceKing
                    {
                        jumpedSquare.Piece = null;
                        targetSquare.Piece.IsKing = true;
                        targetSquare.Piece.Type = PieceType.King;
                        UpdatePieceImage(targetSquare.Piece);
                        hasJumped = true;
                    }

                    else if (jumpedSquare != null && makePieceKing == false)
                    {
                        jumpedSquare.Piece = null;
                        hasJumped = true;
                    }


                }

                CheckForKing();

                ClearHintPositions();

                InitializePiecesCount();

                if (pieseRosu == 0 || pieseAlb == 0)
                {
                    hasJumped = false;
                    EndGame();
                    return;
                }
            }


            else
            {
                // Afișăm un mesaj de eroare sau gestionăm altfel situația în care pătratul țintă este deja ocupat de o piesă a aceluiași jucător
                MessageBox.Show("Acest pătrat este deja ocupat de o piesă a jucătorului tău!");
            }
        }


        private void CheckForKing()
        {
            foreach (var square in GameBoard)
            {
                if (square.Piece != null)
                {
                    if (square.Piece.Color == PieceColor.Red && square.Row == 0)
                    {
                        square.Piece.IsKing = true;
                        square.Piece.Type = PieceType.King;
                        UpdatePieceImage(square.Piece);
                    }
                    else if (square.Piece.Color == PieceColor.White && square.Row == BoardSize - 1)
                    {
                        square.Piece.IsKing = true;
                        square.Piece.Type = PieceType.King;
                        UpdatePieceImage(square.Piece);
                    }
                }
            }
        }


        public void UpdatePieceImage(Piece piece)
        {

            string cale_imagine = piece.GetPieceImagePath();
            Button button = ChessGrid.Children.OfType<Button>().FirstOrDefault(b => b.Content is Image && ((Image)b.Content).Source.ToString() == cale_imagine);

            if (button != null)
            {
                Image pieceImage = new Image();
                pieceImage.Source = new BitmapImage(new Uri(cale_imagine));
                button.Content = pieceImage;
            }
        }


        public void UpdateBoardView()
        {
            SolidColorBrush darkColor = new SolidColorBrush(Color.FromRgb(238, 220, 151));
            SolidColorBrush lightColor = new SolidColorBrush(Color.FromRgb(36, 26, 15));
            SolidColorBrush selectedColor = new SolidColorBrush(Color.FromRgb(255, 255, 0)); // culoarea piesa selectata
            SolidColorBrush hintColor = new SolidColorBrush(Color.FromRgb(0, 255, 0)); // culoarea pentru casetele valide

            foreach (var square in GameBoard)
            {
                // Cautăm butonul corespunzător casetei în ChessGrid.Children
                Button button = ChessGrid.Children.OfType<Button>().FirstOrDefault(b => Grid.GetRow(b) == square.Row && Grid.GetColumn(b) == square.Column);

                if (button != null)
                {
                    button.Background = square.Shade == SquareShade.Dark ? darkColor : lightColor;

                    if (square.Piece != null)
                    {
                        Image pieceImage = new Image();
                        pieceImage.Source = new BitmapImage(new Uri(square.Piece.GetPieceImagePath()));
                        button.Content = pieceImage;
                    }

                    else
                    {
                        // Dacă nu există o piesă, ștergem conținutul butonului
                        button.Content = null;
                    }

                    if (square.IsHint)
                    {
                        button.Background = hintColor;
                    }

                    else if (piesaSelectata != null && square.Piece == piesaSelectata)
                    {
                        button.Background = selectedColor;
                    }
                }
            }
        }


        private void EndGame()
        {
            string winner = pieseRosu > pieseAlb ? "Jucătorul roșu" : (pieseRosu < pieseAlb ? "Jucătorul alb" : "Egalitate");
            int maximPieseRamase = Math.Max(pieseRosu, pieseAlb);

            StatisticsVM.SaveStatistics(winner, maximPieseRamase);
            MessageBox.Show($"Jocul s-a încheiat! {winner} a câștigat!");

            Application.Current.MainWindow.Close();
        }


        public event EventHandler PlayerChanged;


        public PieceColor CurrentPlayer
        {
            get { return currentPlayer; }
            set
            {
                if (currentPlayer != value)
                {
                    currentPlayer = value;
                    OnPropertyChanged("CurrentPlayer");
                    PlayerChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
