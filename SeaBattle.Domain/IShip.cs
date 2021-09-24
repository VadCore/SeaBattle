using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.Domain
{
    public interface IShip
    {
        public int Id { get; set; }
        public int Length { get; }
        public int Range { get; set; }

        public int Speed { get; }

        public int PlayerId { get; }

        public bool IsHorizontal { get; set; }

        public Coordinate Coordinate { get; set; }
    }
}
