// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessInfo.cs" company="Compilyator">
//   All rights reserved
// </copyright>
// <summary>
//   The process info.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TaskManagerDemo
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using TaskManagerDemo.Annotations;

    /// <summary>
    /// The process info.
    /// </summary>
    public class ProcessInfo : INotifyPropertyChanged
    {
        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        [CanBeNull]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [CanBeNull]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        [CanBeNull]
        public string Priority { get; set; }

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CanBeNull] [CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}