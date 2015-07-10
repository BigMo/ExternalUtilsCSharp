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
    public struct PEOptHeader
    {
        public short signature; //decimal number 267 for 32 bit, and 523 for 64 bit.
        public char MajorLinkerVersion;
        public char MinorLinkerVersion;
        public int SizeOfCode;
        public int SizeOfInitializedData;
        public int SizeOfUninitializedData;
        public int AddressOfEntryPoint;  //The RVA of the code entry popublic int
        public int BaseOfCode;
        public int BaseOfData;
        public int ImageBase;
        public int SectionAlignment;
        public int FileAlignment;
        public short MajorOSVersion;
        public short MinorOSVersion;
        public short MajorImageVersion;
        public short MinorImageVersion;
        public short MajorSubsystemVersion;
        public short MinorSubsystemVersion;
        public int Reserved;
        public int SizeOfImage;
        public int SizeOfHeaders;
        public int Checksum;
        public short Subsystem;
        public short DLLCharacteristics;
        public int SizeOfStackReserve;
        public int SizeOfStackCommit;
        public int SizeOfHeapReserve;
        public int SizeOfHeapCommit;
        public int LoaderFlags;
        public int NumberOfRvaAndSizes;
        public data_directory DataDirectory1;
        public data_directory DataDirectory2;
        public data_directory DataDirectory3;
        public data_directory DataDirectory4;
        public data_directory DataDirectory5;
        public data_directory DataDirectory6;
        public data_directory DataDirectory7;
        public data_directory DataDirectory8;
        public data_directory DataDirectory9;
        public data_directory DataDirectory10;
        public data_directory DataDirectory11;
        public data_directory DataDirectory12;
        public data_directory DataDirectory13;
        public data_directory DataDirectory14;
        public data_directory DataDirectory15;
        public data_directory DataDirectory16;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct data_directory
    {
        public int VirtualAddress;
        public int Size;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct _IMAGE_EXPORT_DIRECTORY
    {
        public int Characteristics; //offset 0x0
        public int TimeDateStamp; //offset 0x4
        public short MajorVersion;  //offset 0x8
        public short MinorVersion; //offset 0xa
        public int Name; //offset 0xc
        public int Base; //offset 0x10
        public int NumberOfFunctions;  //offset 0x14
        public int NumberOfNames;  //offset 0x18
        public int AddressOfFunctions; //offset 0x1c
        public int AddressOfNames; //offset 0x20
        public int AddressOfNameOrdinals; //offset 0x24
    }
}
