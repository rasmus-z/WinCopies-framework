using System;

namespace WinCopies.IO
{
    public enum FileType
    {
        None,

        Folder,

        File,

        Drive,

        Link,

        Archive,

        SpecialFolder,

        Other
    }

    [Flags]
    public enum FileTypes
    {
        None = 0,

        Folder = 1,

        File = 2,

        Drive = 4,

        Link = 8,

        Archive = 16
    }
}
