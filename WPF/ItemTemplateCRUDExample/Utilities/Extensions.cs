using System;
using System.Collections.Generic;
using ItemTemplateCRUDExample.Annotations;

namespace ItemTemplateCRUDExample.Utilities
{
    public static class Extensions
    {
        public static void ForEach<T>(
            [NotNull] this IEnumerable<T> source, 
            [NotNull] Action<T> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));
            foreach (var item in source)
            {
                action(item);
            }
        }
    }
}