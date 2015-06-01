﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private MemUtils MemUtils;
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
        public IntPtr COFFHeaderAddress;

        /// <summary>
        /// Address of the PE optional header
        /// </summary>
        public IntPtr PEOptHeaderAddress;

        /// <summary>
        /// Initializes a new PEInfo using the given module
        /// </summary>
        /// <param name="module"></param>
        /// <param name="memUtils">Instance of MemUtils to use in order to read data</param>
        public PEInfo(ProcessModule module, MemUtils memUtils) : this(module.BaseAddress, memUtils) { }

        /// <summary>
        /// Initializes a new PEInfo using the given baseaddress of a module
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <param name="memUtils">Instance of MemUtils to use in order to read data</param>
        public PEInfo(IntPtr baseAddress, MemUtils memUtils)
        {
            MemUtils = memUtils;

            DOSHeader = MemUtils.Read<DOSHeader>(baseAddress);

            COFFHeaderAddress = new IntPtr(baseAddress.ToInt64() + DOSHeader.e_lfanew + 4);
            COFFHeader = MemUtils.Read<COFFHeader>(COFFHeaderAddress);

            PEOptHeaderAddress = new IntPtr(COFFHeaderAddress.ToInt64() + Marshal.SizeOf(typeof(COFFHeader)));
            PEOptHeader = MemUtils.Read<PEOptHeader>(PEOptHeaderAddress);
        }
    }
}
