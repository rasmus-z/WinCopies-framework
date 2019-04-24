﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.GUI.Explorer
{

    public delegate void MultiplePathsOpenRequestedEventHandler(object sender, MultiplePathsOpenRequestedEventArgs e);

    public class MultiplePathsOpenRequestedEventArgs : EventArgs
    {

        /// <summary>
        /// The paths for which an open has been requested.
        /// </summary>
        public IEnumerable<IBrowsableObjectInfo> Paths { get; }

        public MultiplePathsOpenRequestedEventArgs(IEnumerable<IBrowsableObjectInfo> paths) => Paths = paths;

    }
}