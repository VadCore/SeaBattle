﻿using SeaBattle.Application.Services.Interfaces;
using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Interfaces;

namespace SeaBattle.Application.Services
{
	public class PlayerService : BaseService<Player>, IPlayerService
	{
		public PlayerService(IRepository<Player> players) : base(players)
		{
		}

		public Player Create(User user, Board board)
		{
			var player = _entities.Add(new Player(user, board.Id));

			_entities.SaveChanges();

			return player;
		}
	}
}