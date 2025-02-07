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
        [PrimaryKey, Unique]
        public string Id { get; set; }

        [Unique, NotNull]
        public string username {  get; set; }

        [Unique,NotNull]
        public string password { get; set; }
    }
}
