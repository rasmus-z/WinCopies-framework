using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.GUI.Controls
{
    /// <summary>
    /// Values that describe how to underline the links.
    /// </summary>
    public enum LinkUnderliningMode 
    {

        /// <summary>
        /// No underline.
        /// </summary>
        None = 0,

        /// <summary>
        /// Underline only when the mouse is over the link, or when the link is focused.
        /// </summary>
        UnderlineWhenMouseOverOrFocused = 1,

        /// <summary>
        /// Underline only when the mouse is NOT over the link and when the link is NOT focused.
        /// </summary>
        UnderlineWhenNotMouseOverNorFocused = 2,

        /// <summary>
        /// Always underline.
        /// </summary>
        AlwaysUnderline = 3

    }
}
