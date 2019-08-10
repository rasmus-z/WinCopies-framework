using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.Util
{
    public interface IDisposable : System.IDisposable
    {

        bool IsDisposing { get; }

        bool IsDisposed { get; }

    }
}
