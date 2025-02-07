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

        [NotNull, MaxLength(20)]
        public string FirstName { get; set; }

        [NotNull, MaxLength(20)]
        public string LastName { get; set; }

        public int? Age { get; set; }

        [NotNull]
        public int PositionId { get; set; }

        [NotNull, MaxLength(36)]
        public string UserId { get; set; }
    }
}
