using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Linq;

namespace CsAvroGen.DomainModel
{
    public static class Extensions
    {
        private static readonly char _quote = '"';

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToDoubleQoutedString(this string self)
        {
            return _quote + self + _quote;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Repeat(this string self, int count)
        {
            return new StringBuilder(self.Length * count)
                .AppendJoin(self, new string[count + 1])
                .ToString();
        }


        public static Version AssemblyVersion(this Assembly self)
        {
            var attr = CustomAttributeExtensions.GetCustomAttribute<AssemblyFileVersionAttribute>(self);
            return attr == null ? new Version(0, 0, 0, 0) : new Version(attr.Version);
        }
    }
}
