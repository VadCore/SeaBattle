﻿using Microsoft.Extensions.Configuration;
using SeaBattle.Application.Services.Interfaces;
using SeaBattle.Domain.Common;
using SeaBattle.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.UI
{
	public class Application
	{
		private readonly IBoardService _boardService;
		private readonly IPlayerService _playerService;
		private readonly IShipService _shipService;
		private readonly IBattleAbilityService _battleAbilityService;

		public Application(IBoardService boardService,
						   IPlayerService playerService,
						   IShipService shipService, 
						   IBattleAbilityService battleAbilityService)
		{
			_boardService = boardService;
			_playerService = playerService;
			_shipService = shipService;
			_battleAbilityService = battleAbilityService;
		}

		public void Run()
		{
			var board = _boardService.Create(10, 10);

			var player1 = _playerService.Create("Vasya", board);
			var player2 = _playerService.Create("Petya", board);

			var player1BattleMiddleShip = _shipService.CreateBattle(SizeId.MiddleShip, player1, new Coordinate(0, 5, 5), Rotation.Horizontal);

			var player2UniversalHugeShip = _shipService.CreateBattle(SizeId.HugeShip, player2, new Coordinate(0, 1, 5), Rotation.Horizontal);

			_shipService.Relocate(player1BattleMiddleShip, new Coordinate(0, 3, 5), Rotation.Vertical);

			_battleAbilityService.Shot(player2UniversalHugeShip, new Coordinate(0, 3, 6));


			Console.WriteLine(board.Id);

		}
	}
}