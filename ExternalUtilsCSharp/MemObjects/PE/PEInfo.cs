using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.MemObjects.PE
{
    /// <summary>
    /// Parses information about a module
    /// </summary>
    public struct PEInfo
    {
        /// <summary>
        /// DOS-header of the module
        /// </summary>
        public DOSHeader DOSHeader;
        
        /// <summary>
        /// COFF-header of the module
        /// </summary>
        public COFFHeader COFFHeader;
        
        /// <summary>
        /// PE optional header of the module
        /// </summary>
        public PEOptHeader PEOptHeader;

        /// <summary>
        /// Address of the COFF header
        /// </summary>
        public int COFFHeaderAddress;

        /// <summary>
        /// Address of the PE optional header
        /// </summary>
        public int PEOptHeaderAddress;

        /// <summary>
        /// Initializes a new PEInfo using the given file-path
        /// </summary>
        /// <param name="path">Path of the module to parse</param>
        public PEInfo(string path) : this(File.ReadAllBytes(path))
        { }

        /// <summary>
        /// Initializes a new PEInfo using the given data
        /// </summary>
        /// <param name="data">Byte-array containing the content of the module to parse</param>
        public PEInfo(byte[] data)
        {
            MemUtils memUtils = new ExternalUtilsCSharp.MemUtils();
            DOSHeader = memUtils.BytesToT<DOSHeader>(data);

            COFFHeaderAddress = DOSHeader.e_lfanew + 4;
            COFFHeader = memUtils.BytesToT<COFFHeader>(data, COFFHeaderAddress);

            PEOptHeaderAddress = COFFHeaderAddress + Marshal.SizeOf(typeof(COFFHeader));
            PEOptHeader = memUtils.BytesToT<PEOptHeader>(data, PEOptHeaderAddress);
        }

        /// <summary>
        /// Parses a module from memory
        /// </summary>
        /// <param name="module">The module to be parsed</param>
        /// <param name="memUtils">MemUtils-instance that is used to dump the module</param>
        /// <returns></returns>
        public static PEInfo FromMemory(ProcessModule module, MemUtils memUtils)
        {
            return FromMemory(module.BaseAddress, module.ModuleMemorySize, memUtils);
        }

        /// <summary>
        /// Parses a module from memory
        /// </summary>
        /// <param name="baseAddress">The address of the module to be parsed</param>
        /// <param name="length">The size of the module to be parsed</param>
        /// <param name="memUtils">MemUtils-instance that is used to dump the module</param>
        /// <returns></returns>
        public static PEInfo FromMemory(IntPtr baseAddress, int length, MemUtils memUtils)
        {
            byte[] data = new byte[length];
            memUtils.Read(baseAddress, out data, length);
            return new PEInfo(data);
        }

        /// <summary>
        /// Reads the _IMAGE_EXPORT_DIRECTORY of this module from live memory
        /// </summary>
        /// <param name="memUtils">MemUtils-instance that is used to read data</param>
        /// <param name="imageBase">Base-address pf this module in memory</param>
        /// <returns></returns>
        public _IMAGE_EXPORT_DIRECTORY ReadImageExportDirectory(MemUtils memUtils, IntPtr imageBase)
        {
            return memUtils.Read<_IMAGE_EXPORT_DIRECTORY>((IntPtr)(imageBase.ToInt64() + this.PEOptHeader.DataDirectory1.VirtualAddress));
        }

        /// <summary>
        /// Reads the name of this module from live-memory
        /// </summary>
        /// <param name="memUtils">MemUtils-instance that is used to read data</param>
        /// <param name="ied">The _IMAGE_EXPORT_DIRECTORY of this module</param>
        /// <param name="imageBase">Base-address pf this module in memory</param>
        /// <returns></returns>
        public string ReadName(MemUtils memUtils, _IMAGE_EXPORT_DIRECTORY ied, IntPtr imageBase)
        {
            return memUtils.ReadString((IntPtr)(imageBase.ToInt64() + ied.Name), 32, Encoding.ASCII);
        }

        /// <summary>
        /// Reads the Export Address Table (EAT) of this module from live memory
        /// </summary>
        /// <param name="memUtils">MemUtils-instance that is used to read data</param>
        /// <param name="imageBase">Base-address pf this module in memory</param>
        /// <returns></returns>
        public Tuple<string, int>[] ReadExportedFunctions(MemUtils memUtils, IntPtr imageBase)
        {
            return ReadExportedFunctions(memUtils, imageBase, ReadImageExportDirectory(memUtils, imageBase));
        }

        /// <summary>
        /// Reads the Export Address Table (EAT) of this module from live memory
        /// </summary>
        /// <param name="memUtils">MemUtils-instance that is used to read data</param>
        /// <param name="imageBase">Base-address pf this module in memory</param>
        /// <param name="ied">The _IMAGE_EXPORT_DIRECTORY of this module</param>
        /// <returns></returns>
        public Tuple<string, int>[] ReadExportedFunctions(MemUtils memUtils, IntPtr imageBase, _IMAGE_EXPORT_DIRECTORY ied)
        {
            List<Tuple<string, int>> functions = new List<Tuple<string, int>>();
            IntPtr lpFunctions = (IntPtr)(imageBase.ToInt64() + ied.AddressOfFunctions);
            IntPtr lpNames = (IntPtr)(imageBase.ToInt64() + ied.AddressOfNames);
            for (int i = 0; i < ied.NumberOfFunctions; i++)
            {
                int address = memUtils.Read<int>((IntPtr)(lpFunctions.ToInt64() + i * 4));
                string name = "?";
                if (lpFunctions != lpNames)
                {
                    int nameAddress = memUtils.Read<int>((IntPtr)(lpNames.ToInt64() + i * 4));
                    name = memUtils.ReadString((IntPtr)(imageBase.ToInt64() + nameAddress), 64, Encoding.ASCII);
                }
                functions.Add(new Tuple<string, int>(name, address));
            }
            return functions.ToArray();
        }
    }
}
