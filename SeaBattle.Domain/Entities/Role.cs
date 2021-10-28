using SeaBattle.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Domain.Entities
{

    public class Role : BaseEntity<Role>
    {
        public string Name { get; set; }

        public Role(string name)
        {
            Name = name;
        }

        public Role()
        {
        }

        public IList<User> Users { get; set; }
    }
}
