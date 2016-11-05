// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="Compilyator">
//   All rights reserved
// </copyright>
// <summary>
//   The enumerable extentions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TaskManagerDemo
{
    using System;
    using System.Collections.Generic;

    using TaskManagerDemo.Annotations;

    /// <summary>
    /// The enumerable extensions.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// The for each.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <typeparam name="T">
        /// Elements type
        /// </typeparam>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public static void ForEach<T>([NotNull] this IEnumerable<T> source, [NotNull] Action<T> action)
        {
            foreach (var item in source)
            {
                action.Invoke(item);
            }
        }
    }
}