using Microsoft.AspNetCore.Identity;
using SeaBattle.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Domain.Entities
{
    public class User : BaseEntity<User>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public int RoleId { get; set; }
        //public Role Role { get; set; }

        public string Name { get; set; }
        public string NormalizedName { get; set; }

        public IList<Player> Players { get; set; }

        public User(string email, string password, RoleType roleType, string name)
        {
            Email = email;
            Password = password;
            RoleId = (int)roleType;
            Name = name;
        }

        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public User() { }
    }
}
