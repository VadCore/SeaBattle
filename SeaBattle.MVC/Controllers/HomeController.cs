using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IUserService _userService;

        public HomeController(IBoardService boardService,
                           IPlayerService playerService,
                           IShipService shipService,
                           IBattleAbilityService battleAbilityService,
                           IUserService userService)
        {
            _boardService = boardService;
            _playerService = playerService;
            _shipService = shipService;
            _battleAbilityService = battleAbilityService;
            _userService = userService;
        }




        public IActionResult Index()
        {
            return View();
        }

        //[Authorize(Roles = nameof(RoleType.CommonUser))]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
