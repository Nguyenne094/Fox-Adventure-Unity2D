using UnityEngine;

namespace Utilities
{
    public static class Extensions
    {
        public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;

        public static void CheckNullReferences(this Object obj)
        {
            var fields = obj.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            foreach (var field in fields)
            {
                var value = field.GetValue(obj);
                if (value == null)
                {
                    Debug.LogError($"{field.Name} is null in {obj.name}. Please assign it in the inspector.", obj);
                }
            }
        }
    }
}