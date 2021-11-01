﻿using System;

namespace SeaBattle.Domain.Extensions
{
	public static class IntExtensions
	{
		public static int ExceptionIfNotBetweenMinMax(this int value, int min = int.MinValue, int max = int.MaxValue)
		{
			if (value >= min && value <= max)
			{
				return value;
			}
			else
			{
				throw new ArgumentException(string.Format($"Value must be between {0} and {1}", min, max));
			}
		}
	}
}
