using SeaBattle.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.Interfaces
{
    public interface IUnit
    {
        public int Id { get; }
        public int PlayerId { get; }
        public Rotation Rotation { get; set; }
        public Coordinate Coordinate { get; }
        public int Health { get; }

        public int Length { get; }
        public int HealthMax { get; }

        public IAbility Ability { get; }

        public int CalculateDistance(Coordinate to);

        public void Damage(int damage);

        public void Heal(int healShot);

        public bool Allocate(Coordinate to);

        public void Dislocate(Coordinate from);

        public void Dislocate();
    }
}
