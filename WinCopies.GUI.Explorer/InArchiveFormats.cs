using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCopies.GUI.Explorer
{
    [Flags]
    public enum InArchiveFormats
    {

        Zip = 1,

        SevenZip = 2,

        Arj = 4,

        BZip2 = 8,

        Cab = 16,

        Chm = 32,

        Compound = 64,

        Cpio = 128,

        CramFS = 256,

        Deb = 512,

        Dmg = 1024,

        Elf = 2048,

        Fat = 4096,

        Flv = 8192,

        GZip = 16384,

        Hfs = 32768,

        Iso = 65536,

        Lzh = 131072,

        Lzma = 262144,

        Lzma86 = 524288,

        Lzw = 1048576,

        MachO = 2097152,

    }
}
