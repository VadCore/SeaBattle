using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Application.Services.Interfaces
{
    public interface IUserService
    {
        public User Create(string email, string password, RoleType roleType, string name);
        public User GetById(int id);
    }
}
