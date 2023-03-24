using System;

namespace SmallCoder.Extensions
{
    internal static class EnumExtensions
    {
        public static TEnum ToEnum<TEnum>(this string strEnum) where TEnum : struct
        {
            return Enum.TryParse<TEnum>(strEnum, true, out var _enum) ? _enum : default;
        }
    }
}
