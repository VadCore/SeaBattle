using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Interfaces;
using SeaBattle.Infrastructure.Interfaces;
using SeaBattle.Infrastructure.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure
{
    public class SeaBattleSerializationContext : SerializationContext<SeaBattleSerializationContext>, IUnitOfWork
    {
        public SerializationSet<Board> Boards { get; }
        public SerializationSet<Player> Players { get; }
        public SerializationSet<Ship> Ships { get; }
        public SerializationSet<BattleAbility> BattleAbilities { get; }
        public SerializationSet<SupportAbility> SupportAbilities { get; }
        public SerializationSet<CoordinateShip> CoordinateShips { get; }
        public SerializationSet<Size> Sizes { get; } = SeedSizesData();

        [JsonConstructor]
        public SeaBattleSerializationContext()
        {

        }

        public static SerializationSet<Size> SeedSizesData()
        {
            return new SerializationSet<Size>
            {
                new Size{ Id = 1, Title = "SmallShip", Length = 1, HealthMax = 2, Speed = 4, Reloading = 1, Range = 3, DamageShot = 1, HealShot = 1},
                new Size{ Id = 2, Title = "MiddleShip", Length = 3, HealthMax = 4, Speed = 3, Reloading = 1, Range = 4, DamageShot = 2, HealShot = 2},
                new Size{ Id = 3, Title = "BigShip", Length = 5, HealthMax = 7, Speed = 2, Reloading = 2, Range = 3, DamageShot = 3, HealShot = 2},
                new Size{ Id = 4, Title = "HugeShip", Length = 7, HealthMax = 10, Speed = 1, Reloading = 2, Range = 5, DamageShot = 5, HealShot = 3},
            };
        }
    }
}
