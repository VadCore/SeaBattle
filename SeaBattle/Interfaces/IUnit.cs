using SeaBattle.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.Interfaces
{
    public interface IUnit
    {
        public int Id { get; init; }
        public int PlayerId { get; init; }
        public Rotation Rotation { get; init; }
        public Coordinate Coordinate { get; init; }
        public int Health { get; init; }

        public int Length { get; }
        public int HealthMax { get; }

        public IAbility Ability { get; init; }

        public int CalculateDistance(Coordinate to);

        public void Damage(int damage);

        public void Heal(int healShot);

        public bool Allocate(Coordinate to);

        public void Dislocate(Coordinate from);

        public void Dislocate();
    }
}
