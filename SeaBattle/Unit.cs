using SeaBattle.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class Unit
    {
        public int Id { get; }
        public int PlayerId { get; }
        public Rotation Rotation { get; protected set; }
        public Coordinate Coordinate { get; protected set; }

        public int Health { get; protected set; }

        public Unit(int id, int playerId, Rotation rotation, Coordinate coordinate, int health)
        {
            Id = id;
            PlayerId = playerId;
            Rotation = rotation;
            Coordinate = coordinate;
            Health = health;
        }
    }
}
