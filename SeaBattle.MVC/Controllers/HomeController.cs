﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SeaBattle.Application.Services.Interfaces;
using SeaBattle.Domain.Common;
using SeaBattle.Domain.Enums;
using SeaBattle.MVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SeaBattle.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}



        private readonly IBoardService _boardService;
        private readonly IPlayerService _playerService;
        private readonly IShipService _shipService;
        private readonly IBattleAbilityService _battleAbilityService;

        public HomeController(IBoardService boardService,
                           IPlayerService playerService,
                           IShipService shipService,
                           IBattleAbilityService battleAbilityService)
        {
            _boardService = boardService;
            _playerService = playerService;
            _shipService = shipService;
            _battleAbilityService = battleAbilityService;
        }




        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            var board = _boardService.Create(10, 10);

            var player1 = _playerService.Create("Vasya", board);
            var player2 = _playerService.Create("Petya", board);

            var player1BattleMiddleShip = _shipService.CreateBattle(SizeId.MiddleShip, player1, new Coordinate(0, 6, 5), Rotation.Horizontal);

            var player2UniversalHugeShip = _shipService.CreateBattle(SizeId.HugeShip, player2, new Coordinate(0, 1, 5), Rotation.Horizontal);

            _shipService.Relocate(player1BattleMiddleShip, new Coordinate(0, 3, 5), Rotation.Vertical);

            _battleAbilityService.Shot(player2UniversalHugeShip, new Coordinate(0, 6, 5));

            Console.WriteLine("_boardService.ToString(board)");
            Console.WriteLine(_boardService.ToString(board));

            Console.WriteLine("_boardService.GetShipByIndexator");
            Console.WriteLine(_boardService.GetShipByIndexator(board, new Coordinate(0, 1, 5)));

            Console.WriteLine("Test");

            Console.ReadKey();


            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
