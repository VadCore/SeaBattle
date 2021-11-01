using SeaBattle.Application.Services.Interfaces;
using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Enums;
using SeaBattle.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Application.Services
{
    public class UserService : BaseService<User>, IUserService
    {
		public UserService(IRepository<User> users) : base(users){}

		public User Create(string email, string password, RoleType roleType, string name)
		{
			var checkUser = _entities.FindFirst(u => u.Email.ToLower() == email.ToLower());

			if(checkUser == null)
            {
				return null;
            }
			
			var user = _entities.Add(new User(email, password, roleType, name));

			_entities.SaveChanges();

			return user;
		}

		public User GetById(int id)
        {
			return _entities.GetById(id);
        }
	}
}
