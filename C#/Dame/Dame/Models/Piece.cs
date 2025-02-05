using System;
using System.ComponentModel;

namespace Dame.Models
{
    public class Piece : INotifyPropertyChanged
    {
        private string name;
        private PieceColor color;
        private PieceType type;
        private bool isKing;
        private bool hasJumped;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }

        public PieceColor Color
        {
            get { return color; }
            set
            {
                color = value;
                NotifyPropertyChanged(nameof(Color));
            }
        }

        public PieceType Type
        {
            get { return type; }
            set
            {
                type = value;
                NotifyPropertyChanged(nameof(Type));
            }
        }

        public bool IsKing
        {
            get { return isKing; }
            set
            {
                isKing = value;
                NotifyPropertyChanged(nameof(IsKing));
            }
        }

        public Piece(string name, PieceColor color, PieceType type, bool isKing)
        {
            Name = name;
            Color = color;
            Type = type;
            IsKing = isKing;
        }


        public string GetPieceImagePath()
        {
            string cale = "C:\\Facultatea de Matematica si Informatica\\Anul 2\\Semestrul al doilea\\MVP\\Laborator\\L6\\Tema2\\Dame\\Resources\\";

            switch (Color)
            {
                case PieceColor.Red:
                    return IsKing ? cale + "pieceRedKing.png" : cale + "pieceRed.png";
                case PieceColor.White:
                    return IsKing ? cale + "pieceWhiteKing.png" : cale + "pieceWhite.png";
                default:
                    throw new ArgumentException("Invalid piece color.");
            }
        }


        public bool HasJumped
        {
            get { return hasJumped; }
            set
            {
                hasJumped = value;
                NotifyPropertyChanged(nameof(HasJumped));
            }
        }


        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }





    public enum PieceColor
    {
        Red,
        White,
        None 
    }


    public enum PieceType
    {
        Regular,
        King
    }
}

