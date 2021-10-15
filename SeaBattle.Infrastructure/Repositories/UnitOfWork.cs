using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Interfaces;
using SeaBattle.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        [JsonIgnore]
        public IDataHandler DataHandler { get; set; }

        public IRepository<Board> Boards => _boards ??= new Repository<Board>();
        private IRepository<Board> _boards;

        public IRepository<Player> Players => _players ??= new Repository<Player>();
        private IRepository<Player> _players;

        public IRepository<Ship> Ships => _ships ??= new Repository<Ship>();
        private IRepository<Ship> _ships;

        public IRepository<BattleAbility> BattleAbilities => _battleAbilities ??= new Repository<BattleAbility>();
        private IRepository<BattleAbility> _battleAbilities;

        public IRepository<SupportAbility> SupportAbilities => _supportAbilities ??= new Repository<SupportAbility>();
        private IRepository<SupportAbility> _supportAbilities;

        public IRepository<CoordinateShip> CoordinateShips => _coordinateships ??= new Repository<CoordinateShip>();
        private IRepository<CoordinateShip> _coordinateships;

        public IRepository<Size> Sizes => _sizes ??= new Repository<Size>(SeedSizesData());
        private IRepository<Size> _sizes;

        public UnitOfWork(IDataHandler dataHandler)
        {
            DataHandler = dataHandler;
        }

        public void Commit()
        {
            DataHandler.SaveContext(this);
        }

        public static IList<Size> SeedSizesData()
        {
            return new List<Size>
            {
                new Size{ Id = 1, Title = "SmallShip", Length = 1, HealthMax = 2, Speed = 4, Reloading = 1, Range = 3, DamageShot = 1, HealShot = 1},
                new Size{ Id = 2, Title = "MiddleShip", Length = 3, HealthMax = 4, Speed = 3, Reloading = 1, Range = 4, DamageShot = 2, HealShot = 2},
                new Size{ Id = 3, Title = "BigShip", Length = 5, HealthMax = 7, Speed = 2, Reloading = 2, Range = 3, DamageShot = 3, HealShot = 2},
                new Size{ Id = 4, Title = "HugeShip", Length = 7, HealthMax = 10, Speed = 1, Reloading = 2, Range = 5, DamageShot = 5, HealShot = 3},
            };
        }
    }
}

