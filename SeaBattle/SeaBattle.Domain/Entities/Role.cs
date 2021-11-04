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

        public string NormalizedName { get; set; }

        //public IList<User> Users { get; set; }

        public Role(int id, RoleType roleType)
        {
            Id = id;
            Name = roleType.ToString();
            NormalizedName = Name.ToUpperInvariant();
        }

        public Role(){}
    }
}
