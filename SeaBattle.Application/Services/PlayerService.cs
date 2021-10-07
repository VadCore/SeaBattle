using SeaBattle.Application.Services.Interfaces;
using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Application.Services
{
    public class PlayerService : BaseService<Player>, IPlayerService
    {
        public PlayerService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public Player Create(string nick, Board board)
        {
            var player = _unitOfWork.Players.Add(new Player(nick, board.Id));

            _unitOfWork.Commit();

            return player;
        }
    }
}
