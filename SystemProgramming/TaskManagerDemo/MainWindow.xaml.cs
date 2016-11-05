// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Compilyator">
//   All rights reserved
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TaskManagerDemo
{
    using System;

    /// <summary>
    /// Interaction logic for MainWindow
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class. 
        /// </summary>
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
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = new ViewModel(true);
        }
    }
}
