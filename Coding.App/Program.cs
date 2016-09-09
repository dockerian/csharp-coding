// ---------------------------------------------------------------------------
// <copyright file="Program.cs" company="Boathill">
//   Copyright (c) Jason Zhu.  All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------

namespace Coding.App
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// The Program class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
