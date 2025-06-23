using UnityEngine;

namespace Utilities
{
    public static class Extensions
    {
        public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;
    }
}