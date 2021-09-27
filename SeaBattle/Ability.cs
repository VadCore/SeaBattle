using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public abstract class Ability
    {
        public IUnit Unit { get; set; }
        public int RechargedTurn { get; set; }
        public static readonly int rechargingPeriodTurnsCount = 2;
        public abstract int Range { get; }

        private IUnit GetTargetUnit(Coordinate coordinate)
        {
            if (Unit.CalculateDistance(coordinate) > Range)
            {
                Console.WriteLine("The target is too far");

                return null;
            }

            var targetShip = Game.Board[coordinate];

            if (targetShip is null)
            {
                Console.WriteLine("Missed");
            }

            return targetShip;
        }

        public bool Shoot(Coordinate coordinate)
        {
            if (!StartRecharging())
            {
                return false;
            }

            var targetShip = GetTargetUnit(coordinate);
            targetShip?.Damage(((IPossibleBeBattle)Unit).DamageShot);

            return targetShip != null;
        }

        public bool Repair(Coordinate coordinate)
        {
            if (!StartRecharging())
            {
                return false;
            }

            var targetShip = GetTargetUnit(coordinate);
            targetShip?.Heal(((IPossibleBeSupport)Unit).HealShot);

            return targetShip != null;
        }

        private bool StartRecharging()
        {
            if (Game.Turn >= RechargedTurn)
            {
                RechargedTurn = Game.Turn + rechargingPeriodTurnsCount;
            }
            else
            {
                Console.WriteLine("Recharging of this unit isn't complete yet!");
                return false;
            }

            return true;
        }
    }
}
