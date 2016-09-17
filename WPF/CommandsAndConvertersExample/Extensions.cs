using System;

namespace ConvertersDemo
{
    public static class Extensions
    {
        public static bool IsDirty(this string str) => String.IsNullOrEmpty(str) || String.IsNullOrWhiteSpace(str);
    }
}