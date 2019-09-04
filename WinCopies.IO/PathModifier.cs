using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.IO
{
    internal class PathModifier<TParent, TItems, TFactory> : IPathModifier where TParent : class, IBrowsableObjectInfo where TItems : class, IBrowsableObjectInfo where TFactory : IBrowsableObjectInfoFactory

    {

        public bool AreItemsLoaded { set => Accessor.Owner.AreItemsLoaded = value; }

        public BrowsableObjectInfoAccessor<TParent, TItems, TFactory> Accessor { get; }

        IBrowsableObjectInfoAccessor IPathModifier.Accessor => (IBrowsableObjectInfoAccessor)Accessor;

        // public IBrowsableObjectInfo Parent { set => _path.Parent = value; }

        // public IBrowsableObjectInfoCollection<TItems> Items => _path.items;

        // IBrowsableObjectInfoCollection<IBrowsableObjectInfo> IPathModifier.Items => (IBrowsableObjectInfoCollection<IBrowsableObjectInfo>) _path.items ; 

        public PathModifier(BrowsableObjectInfoAccessor<TParent, TItems, TFactory> accessor) => Accessor = accessor;

    }
}
