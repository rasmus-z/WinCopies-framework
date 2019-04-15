using System;

namespace WinCopies.IO
{
    public enum FileType
    {
        None = 0,

        Folder = 1,

        File = 2,

        Drive = 3,

        Link = 4,

        Archive = 5,

        SpecialFolder = 6
    }

    [Flags]
    public enum FileTypesFlags
    {
        None = 0,

        Folder = 1,

        File = 2,

        Drive = 4,

        Link = 8,

        Archive = 16,

        All = 32
    }
}
