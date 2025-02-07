using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace GameSquad.Model
{
    public class User
    {
        [PrimaryKey, Unique, MaxLength(36)]
        public string Id { get; set; }

        [Unique, NotNull, MaxLength(20)]
        public string Username { get; set; }

        [NotNull, MaxLength(20)]
        public string Password { get; set; }
    }
}
