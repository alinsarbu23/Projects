using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace GameSquad.Model
{
    public class Player
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string FirstName { get; set; }

        [NotNull]
        public string LastName { get; set; }

        public int? Age { get; set; }

        [NotNull]
        public int Position { get; set; }

        [NotNull]
        public string UserId { get; set; }
    }
}
