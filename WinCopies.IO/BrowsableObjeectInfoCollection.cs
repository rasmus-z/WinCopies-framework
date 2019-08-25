using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies.Collections;
using WinCopies.Util;
using static WinCopies.Util.Util;

namespace WinCopies.IO
{
    public class ReadOnlyBrowsableObjeectInfoCollection : ReadOnlyObservableCollection<IBrowsableObjectInfo>, IReadOnlyObservableCollection<IBrowsableObjectInfo>
    {

        public ReadOnlyBrowsableObjeectInfoCollection(BrowsableObjectInfoCollection list) : base(list)
        {

        }

        [Serializable]
        public struct Enumerator<TIn, TOut> : IEnumerator<TOut>, IEnumerator
        {

            private IEnumerator<TIn> _enumerator;

            public TOut Current { get; private set; }

            object IEnumerator.Current => Current;

            public Enumerator(IEnumerable<TIn> innerEnumerable)

            {

                _enumerator = innerEnumerable.GetEnumerator();

                Current = default;

            }

            public bool MoveNext()
            {

                if (_enumerator.MoveNext())

                {

                    Current = MoveNextDelegate(_enumerator.Current);

                    return true;

                }

                else

                    return false;

            }

            public void Reset() => Dispose();

            public void Dispose()
            {

                _enumerator.Dispose();

                Current = default;

            }

        }

    }

}
