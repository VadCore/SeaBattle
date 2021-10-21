using System.Reflection;

namespace SeaBattle.Domain.Constants
{
	public static class ReflectionConstants
	{
		public const BindingFlags PublicInstance =
			BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
	}
}
