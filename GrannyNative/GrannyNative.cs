using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using Force.Crc32;

namespace EveExporter.GrannyNative

{
    internal partial class Granny
    {
        private MemoryStream _grannyFile;
        private string _format;

        private GrannyHeader _header;
        private GrannyHeaderInfo _headerInfo;

        private List<SectionHeader> _sectionHeaders = new List<SectionHeader>();
        private List<uint> _sectionOffsets = new List<uint>();

        private byte[] _sectionData = { };


        public Granny(MemoryStream fileStream) { 
            _grannyFile = fileStream;
            ProcessFile();
        }



        private void ProcessFile()
        {
            ReadFormat();
            Debug.WriteLine("GR2 Format: " + _format);
            Debug.Assert(_format.StartsWith("Little"));

            ReadHeader();
            Debug.WriteLine("Header Size: " + _header.Size);

            ReadHeaderInfo();
            Debug.WriteLine("Header Info Version: " + _headerInfo.Version);
            Debug.WriteLine("Header Info File Size: " + _headerInfo.FileSize);
            Debug.WriteLine("Header Info CRC32: " + _headerInfo.CRC32);
            Debug.WriteLine("Header Info Sections Offset: " + _headerInfo.SectionsOffset);
            Debug.WriteLine("Header Info Sections Count: " + _headerInfo.SectionsCount);
            Debug.WriteLine("Header Info Types Section: " + _headerInfo.TypesSection);
            Debug.WriteLine("Header Info Types Offset: " + _headerInfo.TypesOffset);
            Debug.WriteLine("Header Info Root Section: " + _headerInfo.RootSection);
            Debug.WriteLine("Header Info Root Offset: " + _headerInfo.RootOffset);
            Debug.WriteLine("Header Info Tag: " + _headerInfo.Tag);
            Debug.WriteLine("Header Info Extra: " + _headerInfo.Extra);
            Debug.WriteLine("Header Info String Table CRC: " + _headerInfo.StringTableCRC);
            Debug.WriteLine("Header Info Reserved1: " + _headerInfo.Reserved1);
            Debug.WriteLine("Header Info Reserved2: " + _headerInfo.Reserved2);
            Debug.WriteLine("Header Info Reserved3: " + _headerInfo.Reserved3);

            ReadSectionHeaders();

            ReadSectionData();
            Debug.WriteLine("Section Data Size: " + _sectionData.Length);
            Debug.WriteLine("Section Offsets are:");
            foreach (var offset in _sectionOffsets)
            {
                Debug.WriteLine(offset);
            }


            ProcessRelocations();
            System.IO.File.WriteAllBytes(@"c:\debug\debug-relocated.bin", _sectionData);

            //            Only little endian is supported at the moment
            //            ProcessMarshallings();

            ProcessSections();


        }

        // loads data for a member and recursively loads data for its children
        private void ProcessMember(Member member)
        {
            MemoryStream stream = new MemoryStream(_sectionData);
            stream.Position = (long)member.DefinitionOffset;

            if (member.ArraySize > 0)
            {
                throw new NotImplementedException("ArraySize > 0 not implemented");
            }

            if (member.Type == 0)
            {
                return;
            }   

            // Inline
            if (member.Type == 1)
            {
                throw new NotImplementedException("Inline not implemented");
            }
            // Reference
            if (member.Type == 2)
            {
                //throw new NotImplementedException("Reference not implemented");
                ulong offset = ReadPlatformUInt(stream);
                Debug.WriteLine("Processing member type 2 Reference with offset " + offset);

                if (offset == 0)
                {
                    return;
                }

                    

            }

            // 3 == Reference to Array
            // 7 == Reference to Variant Array
            if (member.Type == 3 || member.Type == 7)
            {
                throw new NotImplementedException("Reference to Array not implemented");
            }   

            // 4 == Array of references to members
            if (member.Type == 4)
            {
                throw new NotImplementedException("Array of references not implemented");
            }

            // 5 == extended data
            if (member.Type == 5)
            {
                throw new NotImplementedException("Extended data not implemented");
            }

            // 8 == string
            if (member.Type == 8)
            {
                throw new NotImplementedException("String not implemented");
            }

            // 9 == Transform Data
            if (member.Type == 9)
            {
                throw new NotImplementedException("Transform Data not implemented");
            }

            // 10 == Float
            if (member.Type == 10)
            {
                throw new NotImplementedException("Float not implemented");
            }

            // 11 or 13 == byte/char
            if (member.Type == 11 || member.Type == 13)
            {
                throw new NotImplementedException("Byte not implemented");
            }

            // 12 or 14 == byte/unsigned char
            if (member.Type == 12 || member.Type == 14)
            {
                throw new NotImplementedException("Unsigned Byte not implemented");
            }

            // 15 or 17 == short/int16
            if (member.Type == 15 || member.Type == 17)
            {
                throw new NotImplementedException("Short not implemented");
            }

            // 16 or 18 == unsigned short/uint16
            if (member.Type == 16 || member.Type == 18)
            {
                throw new NotImplementedException("Unsigned Short not implemented");
            }

            // 19 == int
            if (member.Type == 19) {
                throw new NotImplementedException("Int not implemented");
            }

            // 20 == unsigned int
            if (member.Type == 20)
            {
               throw new NotImplementedException("Unsigned Int not implemented");
            }

            //21 == float16
            if (member.Type == 21)
            {
                throw new NotImplementedException("Float16 not implemented");
            }


        }


        private void ProcessSections()
        {
            RootObject rootObject = new RootObject();
            rootObject.Offset = _sectionOffsets[(int)_headerInfo.TypesSection] + _headerInfo.TypesOffset;
            MemoryStream dataStream = new MemoryStream(_sectionData);
            dataStream.Position = rootObject.Offset;

            Debug.WriteLine("Root Object Offset: " + rootObject.Offset);

            Member[] members = ReadMembers(dataStream);

            foreach (var member in members)
            {
                ProcessMember(member);
            }

        }

        // reads an array of members (until a member with type 0 is found)
        private Member[] ReadMembers(MemoryStream dataStream)
        {
            List<Member> members = new List<Member>();

            while (true)
            {
                Member m = ReadMember(dataStream);
                if (m.Type == 0)
                {
                    break;
                }
                members.Add(m);
            }
            return members.ToArray();
        }

        // reads a single member from the data stream
        private Member ReadMember(MemoryStream dataStream)
        {

            Member m = new Member();

            Debug.WriteLine("Reading Member at " + dataStream.Position);

            m.Type = ReadUInt32(dataStream);

            Debug.WriteLine("Member Type is " + m.Type);
            if (m.Type == 0)
            {
                return m;
            }

            if (_format.ToString().EndsWith("32")) { m.StringOffset = ReadUInt32(dataStream); } else { m.StringOffset = ReadUInt64(dataStream); }
            if (_format.ToString().EndsWith("32")) { m.DefinitionOffset = ReadUInt32(dataStream); } else { m.DefinitionOffset = ReadUInt64(dataStream); }

            m.ArraySize = ReadUInt32(dataStream);

            Debug.WriteLine("Member String Offset is " + m.StringOffset);
            Debug.WriteLine("Member Definition Offset is " + m.DefinitionOffset);
            Debug.WriteLine("Member Array Size is " + m.ArraySize);

            for (int i = 0; i < 3; i++)
            {
                m.Extra.Append(ReadUInt32(dataStream));
            }
            Debug.WriteLine("Member Extra is " + m.Extra);


            // not sure why we are throwing these bytes away
            if (_format.ToString().EndsWith("32")) { ReadUInt32(dataStream); } else { ReadUInt64(dataStream); }

            // save the current position
            long currentPosition = dataStream.Position;

            // move to the string offset
            dataStream.Position = (long)m.StringOffset;

            // read the string
            m.Name = ReadString(dataStream);
            Debug.WriteLine(" read " + m.Name.Length + " bytes");
            Debug.WriteLine(" name is [" + m.Name + "]");

            // move back to the saved position
            dataStream.Position = currentPosition;

            return m;
        }


        private void ReadFormat()
        {
            _grannyFile.Position = 0;
            byte[] ByteArray = ReadByteArray(_grannyFile, 16);

            if (ByteArray.SequenceEqual(LittleEndian32v1) || ByteArray.SequenceEqual(LittleEndian32v2)||
                ByteArray.SequenceEqual(LittleEndian32v3) || ByteArray.SequenceEqual(LittleEndian32v4))
            {
                _format = "LittleEndian32";
            }

            else if (ByteArray.SequenceEqual(LittleEndian64v1) || ByteArray.SequenceEqual(LittleEndian64v2))
            {
                _format = "LittleEndian64";
            }


            // TODO:   Add support for BigEndian formats
            //else if (ReadByteArray.SequenceEqual(BigEndian32v1) || ReadByteArray.SequenceEqual(BigEndian32v2) ||
            //                   ReadByteArray.SequenceEqual(BigEndian32v3))
            //{
            //    format = "BigEndian32";
            //}

            //else if (ReadByteArray.SequenceEqual(BigEndian64v1) || ReadByteArray.SequenceEqual(BigEndian64v2))
            //{
            //    format = "BigEndian64";
            //}


            else
            {
                throw new Exception("Unknown format");
            }

        }

        private void ReadHeader()
        {
            _header = new GrannyHeader();
            _grannyFile.Position = 0;
            _header.Magic = ReadByteArray(_grannyFile, 16);
            _header.Size = ReadUInt32(_grannyFile);
            _header.Format = ReadUInt32(_grannyFile);
            _header.Reserved = ReadByteArray(_grannyFile, 8);
        }

        private void ReadHeaderInfo()
        {
            _headerInfo = new GrannyHeaderInfo();
            _grannyFile.Position = 32;

            _headerInfo.Version = ReadUInt32(_grannyFile);
            _headerInfo.FileSize = ReadUInt32(_grannyFile);
            _headerInfo.CRC32 = ReadUInt32(_grannyFile);
            _headerInfo.SectionsOffset = ReadUInt32(_grannyFile);
            _headerInfo.SectionsCount = ReadUInt32(_grannyFile);
            _headerInfo.TypesSection = ReadUInt32(_grannyFile);
            _headerInfo.TypesOffset = ReadUInt32(_grannyFile);
            _headerInfo.RootSection = ReadUInt32(_grannyFile);
            _headerInfo.RootOffset = ReadUInt32(_grannyFile);
            _headerInfo.Tag = ReadUInt32(_grannyFile);

            _headerInfo.Extra = ReadByteArray(_grannyFile, 16);

            if (_headerInfo.Version == 7)
            {

                _headerInfo.StringTableCRC = ReadUInt32(_grannyFile);
                _headerInfo.Reserved1 = ReadUInt32(_grannyFile);
                _headerInfo.Reserved2 = ReadUInt32(_grannyFile);
                _headerInfo.Reserved3 = ReadUInt32(_grannyFile);
            }

            // check the crc
            uint headersize = (uint)_grannyFile.Position;
            uint filesize = (uint)_grannyFile.Length;

            // read all remaining bytes and calculate the CRC32 of them
            int bytecount = (int)(filesize - headersize);
            byte[] filebytes = ReadByteArray(_grannyFile, bytecount);
            uint crc32 = Crc32Algorithm.Compute(filebytes);

            if (crc32 != _headerInfo.CRC32)
            {
                throw new Exception("CRC32 mismatch");
            }
            
            _grannyFile.Position = headersize;

        }

        private void ReadSectionHeaders()
        {

            for (int i = 0; i < _headerInfo.SectionsCount; i++)
            {
                SectionHeader sectionHeader = new SectionHeader();
                sectionHeader.Compression = ReadUInt32(_grannyFile);
                sectionHeader.DataOffset = ReadUInt32(_grannyFile);
                sectionHeader.DataSize = ReadUInt32(_grannyFile);
                sectionHeader.DecompressedSize = ReadUInt32(_grannyFile);
                sectionHeader.Alignment = ReadUInt32(_grannyFile);
                sectionHeader.First16Bit = ReadUInt32(_grannyFile);
                sectionHeader.First8Bit = ReadUInt32(_grannyFile);
                sectionHeader.RelocationsOffset = ReadUInt32(_grannyFile);
                sectionHeader.RelocationsCount = ReadUInt32(_grannyFile);
                sectionHeader.MarshallingsOffset = ReadUInt32(_grannyFile);
                sectionHeader.MarshallingsCount = ReadUInt32(_grannyFile); 
                _sectionHeaders.Add(sectionHeader);

                Debug.WriteLine("Section " + i + " ======================================================");
                Debug.WriteLine("Section " + i + " Compression: " + sectionHeader.Compression);
                Debug.WriteLine("Section " + i + " Data Offset: " + sectionHeader.DataOffset);
                Debug.WriteLine("Section " + i + " Data Size: " + sectionHeader.DataSize);
                Debug.WriteLine("Section " + i + " Decompressed Size: " + sectionHeader.DecompressedSize);
                Debug.WriteLine("Section " + i + " Alignment: " + sectionHeader.Alignment);
                Debug.WriteLine("Section " + i + " First 16 Bit: " + sectionHeader.First16Bit);
                Debug.WriteLine("Section " + i + " First 8 Bit: " + sectionHeader.First8Bit);
                Debug.WriteLine("Section " + i + " Relocations Offset: " + sectionHeader.RelocationsOffset);
                Debug.WriteLine("Section " + i + " Relocations Count: " + sectionHeader.RelocationsCount);
                Debug.WriteLine("Section " + i + " Marshallings Offset: " + sectionHeader.MarshallingsOffset);
                Debug.WriteLine("Section " + i + " Marshallings Count: " + sectionHeader.MarshallingsCount);
                Debug.WriteLine("\n");

            }

        }

        //private void GetMemberData(MemoryStream stream, Member[] members, Member parent)
        //{

        //    foreach (Member member in members)
        //    {

        //        if (member.ArraySize > 0)
        //        {
        //            Debug.WriteLine("Member " + member.Name + " Array Size: " + member.ArraySize);
        //            float converToFloat = 1;


        //            if ((member.Type == 15 || member.Type == 17) && member.Name == "Position")
        //            {

        //            }

        //    }


        //}


        // Goes through all the sections decompresses and concantinates them into _sectionData
        private void ReadSectionData()
        {
            uint currentOffset = 0;

            // for each section header read the section
            foreach (SectionHeader sectionHeader in _sectionHeaders)
            {
                _grannyFile.Position = sectionHeader.DataOffset;
                byte[] compressedSectionData = ReadByteArray(_grannyFile, (int)sectionHeader.DataSize);
                byte[] sectionData = new byte[sectionHeader.DecompressedSize];

                _sectionOffsets.Add(currentOffset);

                if (sectionHeader.Compression == 0)
                {
                    sectionData = compressedSectionData;
                }
                else if ((sectionHeader.Compression >= 1) && (sectionHeader.Compression <= 4))
                {
                    if (sectionHeader.DataSize == sectionHeader.DecompressedSize)
                    {
                        sectionData = compressedSectionData;
                    }
                    else
                    {
                        Decompression decompressor = new Decompression();
                        decompressor.GR2decompress(sectionData, compressedSectionData, sectionHeader.DecompressedSize, sectionHeader.DataSize, sectionHeader);
                        Debug.WriteLine("Decompressed Size: " + sectionHeader.DecompressedSize);
                    }
                }
                else
                {
                    throw new Exception("Unknown compression type " + sectionHeader.Compression);
                }

                if (sectionHeader.DecompressedSize > 0)
                {
                    currentOffset += sectionHeader.DecompressedSize;
                    _sectionData = _sectionData.Concat(sectionData).ToArray();
                }

            }

        }

        private void ProcessRelocations()
        {
            int sectionIndex = 0;
            byte[] decompressedRelocationData;

            // for each section header read the section
            foreach (SectionHeader sectionHeader in _sectionHeaders)
            {
                if (sectionHeader.RelocationsCount == 0)
                {
                    sectionIndex++;
                    continue;
                }

                _grannyFile.Position = sectionHeader.RelocationsOffset;
                decompressedRelocationData = new byte[sectionHeader.RelocationsCount * 12];


                bool decompressionRequired = true;
                if ((sectionHeader.MarshallingsOffset - sectionHeader.RelocationsOffset) == (sectionHeader.RelocationsCount*12)) {
                    decompressionRequired = false;
                }

                if (!decompressionRequired)
                {
                    decompressedRelocationData = ReadByteArray(_grannyFile, (int)sectionHeader.RelocationsCount * 12);
                }
                else
                {
                    uint compressedSize = ReadUInt32(_grannyFile);
                    byte[] compressedRelocationData = ReadByteArray(_grannyFile, (int)compressedSize);
                    Decompression decompressor = new Decompression();
                    decompressor.GR2decompress(decompressedRelocationData, compressedRelocationData, sectionHeader.RelocationsCount * 12, compressedSize, sectionHeader);
                }

                // Apply all of the relocations
                for (int i = 0; i < sectionHeader.RelocationsCount; i++)
                {
                    Relocation relocation = new Relocation();
                    relocation.Offset = BitConverter.ToUInt32(decompressedRelocationData, i * 12);
                    relocation.TargetSection = BitConverter.ToUInt32(decompressedRelocationData, i * 12 + 4);
                    relocation.TargetOffset = BitConverter.ToUInt32(decompressedRelocationData, i * 12 + 8);

                    // copy 4 bytes from the target section and offset to the offset in this section

                    uint targetOffset = _sectionOffsets[(int)relocation.TargetSection] + relocation.TargetOffset;
                    byte[] targetOffsetBytes = BitConverter.GetBytes(targetOffset);

//                    targetOffsetBytes = targetOffsetBytes.Reverse().ToArray();

                    uint destinationOffset = _sectionOffsets[sectionIndex] + relocation.Offset;

                    if (destinationOffset > 1089295)
                    {
                        Debug.WriteLine("****: Relocation " + i + " Destination: " + destinationOffset + " Target: " + targetOffset + " Bytes: " + targetOffsetBytes);
                    }

                    Array.Copy(targetOffsetBytes, 0 , _sectionData, destinationOffset, 4);
//                    Array.Copy(_sectionData, targetOffset, _sectionData, destinationOffset, 4);
                }

                sectionIndex++;
            }
        }


        // TODO: apparently marshallings are only used on BigEndian formatted files so this needs to be tested
        private void ProcessMarshallings()
        {
            int sectionIndex = 0;

            // for each section header read the section
            foreach (SectionHeader sectionHeader in _sectionHeaders)
            {
                if (sectionHeader.MarshallingsCount == 0)
                {
                    sectionIndex++;
                    continue;
                }

                _grannyFile.Position = sectionHeader.MarshallingsOffset;

                uint compressedSize = ReadUInt32(_grannyFile);
                byte[] compressedMarshallingsData = ReadByteArray(_grannyFile, (int)compressedSize);
                byte[] decompressedMarshallingsData = new byte[sectionHeader.MarshallingsCount * 12];

                if (sectionHeader.Compression == 0)
                {
                    decompressedMarshallingsData = compressedMarshallingsData;
                }
                else if (sectionHeader.Compression == 4)
                {
                    if (compressedSize == sectionHeader.MarshallingsCount * 12)
                    {
                        decompressedMarshallingsData = compressedMarshallingsData;
                    }
                    else
                    {
                        Decompression decompressor = new Decompression();
                        decompressor.GR2decompress(decompressedMarshallingsData, compressedMarshallingsData, sectionHeader.MarshallingsCount * 12, compressedSize, sectionHeader);
                    }
                }
                else
                {
                    throw new Exception("Unknown compression type " + sectionHeader.Compression);
                }
            }
        }



        private string ReadString(MemoryStream Stream)
        {
            String s = "";

//            Debug.WriteLine("Reading string at Position: " + Stream.Position);

            while (true)
            {
                byte[] ReadByteArray = new byte[1];
                Stream.Read(ReadByteArray, 0, 1);
                if (ReadByteArray[0] == 0)
                {
                    break;
                }
                s += Encoding.ASCII.GetString(ReadByteArray);
            }
            return s;
        }


        // reads a 32 bit uint from 32 bit files and a 64 bit ulong from 64 bit files
        private ulong ReadPlatformUInt(MemoryStream Stream)
        {

            if (_format.EndsWith("64"))
            {
                return ReadUInt64(Stream);
            }

            else
            {
                return ReadUInt32(Stream);
            }   

        }

        private uint ReadUInt32(MemoryStream Stream)
        {
            byte[] ReadByteArray = new byte[4];
            Stream.Read(ReadByteArray, 0, 4);
            uint data = BitConverter.ToUInt32(ReadByteArray, 0);
            return data;
        }
        private ulong ReadUInt64(MemoryStream Stream)
        {
            byte[] ReadByteArray = new byte[8]; 
            Stream.Read(ReadByteArray, 0, 8);
            ulong data = BitConverter.ToUInt32(ReadByteArray, 8);
            return data;
        }

        private byte[] ReadByteArray(MemoryStream Stream, int size)
        {
            byte[] ByteArray = new byte[size];
            Stream.Read(ByteArray, 0, size);
            return ByteArray;
        }


    }
}
