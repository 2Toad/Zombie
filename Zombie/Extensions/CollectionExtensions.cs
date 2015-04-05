using System;
using System.Collections.Generic;

namespace Zombie.Extensions
{
    public static class CollectionExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection) action.Invoke(item);
        }
    }
}
