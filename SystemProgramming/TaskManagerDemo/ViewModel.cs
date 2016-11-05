// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewModel.cs" company="Compilyator">
//   All rights reserved
// </copyright>
// <summary>
//   The view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TaskManagerDemo
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Threading;

    using TaskManagerDemo.Annotations;

    /// <summary>
    /// The view model.
    /// </summary>
    public class ViewModel : INotifyPropertyChanged
    {
        private DateTime lastUpdated;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        /// <param name="start">
        /// The start.
        /// </param>
        /// <exception cref="OverflowException">
        /// <paramref>
        ///     <name>value</name>
        /// </paramref>
        /// is less than <see cref="F:System.TimeSpan.MinValue"/> or greater than <see cref="F:System.TimeSpan.MaxValue"/>.-or-<paramref>
        ///         <name>value</name>
        ///     </paramref>
        ///     is <see cref="F:System.Double.PositiveInfinity"/>.-or-<paramref>
        ///         <name>value</name>
        ///     </paramref>
        /// is <see cref="F:System.Double.NegativeInfinity"/>. 
        /// </exception>
        public ViewModel(bool start)
        {
            this.Processes = new ObservableCollection<ProcessInfo>();
            var timer = new DispatcherTimer(DispatcherPriority.Background) { Interval = TimeSpan.FromSeconds(5) };
            timer.Tick += this.UpdateProcesses;
            if (start)
            {
                timer.Start();
            }
        }

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the processes.
        /// </summary>
        [NotNull]
        public ObservableCollection<ProcessInfo> Processes { get; set; }

        /// <summary>
        /// Gets or sets the last updated.
        /// </summary>
        /// <exception cref="Exception" accessor="set">A delegate callback throws an exception.</exception>
        public DateTime LastUpdated
        {
            get
            {
                return this.lastUpdated;
            }

            set
            {
                if (value.Equals(this.lastUpdated))
                {
                    return;
                }
                this.lastUpdated = value;
                this.OnPropertyChanged();
            }
        }

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

        /// <summary>
        /// The get processes.
        /// </summary>
        private void GetProcesses()
        {
            this.Processes.Clear();
            Process.GetProcesses().Select(
                process =>
                    {
                        try
                        {
                            return new ProcessInfo
                                       {
                                           Id = process.Id,
                                           Name = process.ProcessName,
                                           FileName = process.MainModule.FileName,
                                           Priority = process.PriorityClass.ToString()
                                       };
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }).Where(process => process != null).ForEach(process => this.Processes.Add(process));
        }

        /// <summary>
        /// The update processes.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="eventArgs">
        /// The event Args.
        /// </param>
        private void UpdateProcesses([NotNull] object sender, [NotNull] EventArgs eventArgs)
        {
            this.GetProcesses();
            this.LastUpdated = DateTime.Now;
        }

        /// <summary>
        /// The remove exists.
        /// </summary>
        /// <param name="processes">
        /// The processes.
        /// </param>
        private void RemoveNotExists([NotNull] List<ProcessInfo> processes)
        {
            this.Processes.Where(e => processes.All(x => x.Id != e.Id)).ToList().ForEach(e => this.Processes.Remove(e));
        }

        /// <summary>
        /// The add new.
        /// </summary>
        /// <param name="processes">
        /// The processes.
        /// </param>
        private void AddNew([NotNull] List<ProcessInfo> processes)
        {
            processes.ForEach(
                process =>
                    {
                        var existProcess = this.Processes.SingleOrDefault(e => e.Id == process.Id);
                        if (existProcess != null)
                        {
                            existProcess.Priority = process.Priority;
                        }
                        else
                        {
                            this.Processes.Add(process);
                        }
                    });
        }
    }
}