using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.Util
{
    public delegate T DeepClone<T>(bool? preserveIds);

    public interface IDeepCloneable
    {

        bool NeedsObjectsReconstruction { get; }

        object DeepClone(bool? preserveIds);

    }
}
