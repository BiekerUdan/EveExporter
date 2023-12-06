using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveExporter.GrannyNative
{
    internal class GrannyHeader
    {
        public byte[] Magic = { }; // 16 bytes
        public uint Size = 0;
        public uint Format = 0;
        public byte[] Reserved = { };
    }

    internal class GrannyHeaderInfo
    {
        public uint Version = 0;
        public uint FileSize = 0;
        public uint CRC32 = 0;
        public uint SectionsOffset = 0;
        public uint SectionsCount = 0;
        public uint TypesSection = 0;
        public uint TypesOffset = 0;
        public uint RootSection = 0;
        public uint RootOffset = 0;
        public uint Tag = 0;
        public byte[] Extra = { }; // 16 bytes
        public uint StringTableCRC = 0;
        public uint Reserved1 = 0;
        public uint Reserved2 = 0;
        public uint Reserved3 = 0;
    }

    internal class SectionHeader
    {
        public uint Compression = 0;
        public uint DataOffset = 0;
        public uint DataSize = 0;
        public uint DecompressedSize = 0;
        public uint Alignment = 0;
        public uint First16Bit = 0;
        public uint First8Bit = 0;
        public uint RelocationsOffset = 0;
        public uint RelocationsCount = 0;
        public uint MarshallingsOffset = 0;
        public uint MarshallingsCount = 0;

        public byte[] Data = { };
    
    }

    internal class Relocation
    {
        public uint Offset = 0;
        public uint TargetSection = 0;
        public uint TargetOffset = 0;
    }

    internal class Marshalling
    {
        public uint Offset = 0;
        public uint Section = 0;
        public uint Type = 0;
        public uint Index = 0;
    }

    internal class RootObject
    {
        public uint Section = 0;
        public uint Offset = 0;
    }

    internal class Member
    {
        public string Name = "";
        public uint Type = 0;
        public ulong DefinitionOffset = 0;
        public ulong StringOffset = 0;
        public uint ArraySize = 0;
        public uint[] Extra = Array.Empty<uint>();
        public uint Unknown = 0;
        public uint[] Data = Array.Empty<uint>();
    }

}

