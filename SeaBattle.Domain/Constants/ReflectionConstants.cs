using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Domain.Constants
{
    public static class ReflectionConstants
    {
        public const BindingFlags PublicInstance =
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
    }
}
