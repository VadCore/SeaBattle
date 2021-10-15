using SeaBattle.Domain.Entities;
using SeaBattle.Infrastructure.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure
{
    public class SeaBattleContext : ORMContext<SeaBattleContext>
    {

        public DataSet<Board> Boards { get; }

            public DataSet<Player> Players { get; }

            public DataSet<Ship> Ships { get; }

            public DataSet<BattleAbility> BattleAbilities { get; }

            public DataSet<SupportAbility> SupportAbilities { get; }

            public DataSet<CoordinateShip> CoordinateShips { get; }

            public DataSet<Size> Sizes { get; } //= SeedSizesData();

            public SeaBattleContext(string connectionString) : base(connectionString)
            {
            }

        //public static DataSet<Size> SeedSizesData()
        //    {
        //        return new DataSet<Size>
        //            {
        //            new Size{ Id = 1, Title = "SmallShip", Length = 1, HealthMax = 2, Speed = 4, Reloading = 1, Range = 3, DamageShot = 1, HealShot = 1},
        //            new Size{ Id = 2, Title = "MiddleShip", Length = 3, HealthMax = 4, Speed = 3, Reloading = 1, Range = 4, DamageShot = 2, HealShot = 2},
        //            new Size{ Id = 3, Title = "BigShip", Length = 5, HealthMax = 7, Speed = 2, Reloading = 2, Range = 3, DamageShot = 3, HealShot = 2},
        //            new Size{ Id = 4, Title = "HugeShip", Length = 7, HealthMax = 10, Speed = 1, Reloading = 2, Range = 5, DamageShot = 5, HealShot = 3},
        //            };
        //    }
    }
}
