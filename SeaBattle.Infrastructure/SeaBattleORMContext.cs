using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Interfaces;
using SeaBattle.Infrastructure.ORM;
using SeaBattle.UI.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure
{
    public class SeaBattleORMContext : ORMContext<SeaBattleORMContext>, IUnitOfWork
    {

        public ORMSet<Board> Boards { get; set; }
        public ORMSet<Player> Players { get; set; }
        public ORMSet<Ship> Ships { get; set; }
        public ORMSet<BattleAbility> BattleAbilities { get; set; }
        public ORMSet<SupportAbility> SupportAbilities { get; set; }
        public ORMSet<CoordinateShip> CoordinateShips { get; set; }
        public ORMSet<Size> Sizes { get; set; }

        public SeaBattleORMContext(IAppOptions options) : base(options.DbConnectionString)
        {

            Configure<Board>().WithNavigation(b=> b.CoordinateShips)
                              .ByForeignKey(cs=> cs.BoardId);

            Configure<Board>().WithNavigation(b => b.Players)
                              .ByForeignKey(p=> p.BoardId);

            Configure<CoordinateShip>().WithNavigation(cs => cs.Ship)
                                       .ByForeignKey(cs => cs.ShipId);
        }
    }
}
