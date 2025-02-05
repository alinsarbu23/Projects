using System.ComponentModel;

namespace Dame.Models
{
    public class GameSquare : INotifyPropertyChanged
    {
        private int row;
        private int column;

        private SquareShade nuanta;
        private Piece piece;

        private bool isHint; //pe o pozitie
        private bool isExtraHint; //saritura multipla

        public GameSquare(int row, int column, SquareShade shade, Piece piece)
        {
            Row = row;
            Column = column;
            Shade = shade;
            Piece = piece;
            IsHint = false;
        }

        public int Row
        {
            get { return row; }
            set
            {
                row = value;
                OnPropertyChanged(nameof(Row));
            }
        }

        public int Column
        {
            get { return column; }
            set
            {
                column = value;
                OnPropertyChanged(nameof(Column));
            }
        }

        public SquareShade Shade
        {
            get { return nuanta; }
            set
            {
                nuanta = value;
                OnPropertyChanged(nameof(Shade));
            }
        }

        public Piece Piece
        {
            get { return piece; }
            set
            {
                piece = value;
                OnPropertyChanged(nameof(Piece));
            }
        }

        public bool IsHint 
        {
            get { return isHint; }
            set
            {
                isHint = value;
                OnPropertyChanged(nameof(IsHint));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsExtraHint 
        {
            get { return isExtraHint; }
            set
            {
                isExtraHint = value;
                OnPropertyChanged(nameof(IsExtraHint));
            }
        }


    }
}

