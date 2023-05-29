using UnityEngine;

namespace Helpers
{
    public static class StringExtensions
    {
        public static string Red(this string str) => $"<color=#F00>{str}</color>";
        public static string Green(this string str) => $"<color=#0F0>{str}</color>";
        public static string Yellow(this string str) => $"<color=#FF0>{str}</color>";
    }
}
