using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.MemObjects.PE
{
    /// <summary>
    /// Source: https://en.wikibooks.org/wiki/X86_Disassembly/Windows_Executable_Files#Code_Sections
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct DOSHeader
    {
        public short signature;        
        public short lastsize;
        public short nblocks;
        public short nreloc;
        public short hdrsize;
        public short minalloc;
        public short maxalloc;
        public short ss;
        public short sp;
        public short checksum;
        public short ip;
        public short cs;
        public short relocpos;
        public short noverlay;

        public short reserved1;
        public short reserved2;
        public short reserved3;
        public short reserved4;

        public short oem_id;
        public short oem_info;

        public short reserved5;
        public short reserved6;
        public short reserved7;
        public short reserved8;
        public short reserved9;
        public short reserved10;
        public short reserved11;
        public short reserved12;
        public short reserved13;
        public short reserved14;

        public int e_lfanew;
    }
}
