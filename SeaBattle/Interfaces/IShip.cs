using SeaBattle.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.Interfaces
{
    public interface IShip
    {
        public int Id { get; set; }
        public int Length { get; }
        public int Range { get; set; }
        public int Speed { get; }
        public int PlayerId { get; }
        public Rotation Rotation { get; set; }
        public Coordinate Coordinate { get; set; }

        public Ship GetTargetShip(Coordinate coordinate);

        public void Damage(int damage);

        public void Heal(int healShot);

        public int CalculateDistance(Coordinate to);

        public bool Allocate(Coordinate to);

        public void Dislocate(Coordinate from);

        public void Dislocate();

        public bool Relocate(Coordinate from, Coordinate to);

        public bool Rotate(Rotation target);

    }
}
