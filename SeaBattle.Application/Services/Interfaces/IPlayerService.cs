using SeaBattle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Application.Services.Interfaces
{
	public interface IPlayerService
	{
		public Player Create(string nick, Board board);
	}
}
