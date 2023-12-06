using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EveExporter
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DdsPixelFormat
    {
        public uint DwSize;
        public uint DwFlags;
        public uint DwFourCC;
        public uint DwRGBBitCount;
        public uint DwRBitMask;
        public uint DwGBitMask;
        public uint DwBBitMask;
        public uint DwABitMask;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DdsHeader
    {
        public uint DwSize;
        public uint DwFlags;
        public uint DwHeight;
        public uint DwWidth;
        public uint DwPitchOrLinearSize;
        public uint DwDepth;
        public uint DwMipMapCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
        public uint[] DwReserved1;
        public DdsPixelFormat Ddspf;
        public uint DwCaps;
        public uint DwCaps2;
        public uint DwCaps3;
        public uint DwCaps4;
        public uint DwReserved2;
    }

    public struct DdsHeaderDX10
    {
        public uint DxgiFormat;
        public uint ResourceDimension;
        public uint MiscFlag;
        public uint ArraySize;
        public uint MiscFlags2;
    }



    internal class DDSFile
    {

        public const uint DXT1 = 0x31545844;
        public const uint DXT2 = 0x32545844;
        public const uint DXT3 = 0x33545844;
        public const uint DXT4 = 0x34545844;
        public const uint DXT5 = 0x35545844;
        public const uint DX10 = 0x30315844;
        public const uint ATI1 = 0x31495441;
        public const uint ATI2 = 0x32495441;



        public DdsHeader header;
        public DdsHeaderDX10 dx10header;
        public Boolean isDX10 = false;

        // construct a DDSFile object by pointing the constructor at a filename
        public DDSFile(string path)
        {
            using (var fileStream = new System.IO.FileStream(path, System.IO.FileMode.Open))
            {
                header = ReadDDSHeader(fileStream);

                if (header.Ddspf.DwFourCC == 0x30315844)
                {
                    dx10header = ReadDX10Header(fileStream);
                    isDX10 = true;
                }
            }

        }

        public void DumpHeaders()
        {
            Debug.WriteLine("DDSFile: Width = " + header.DwWidth);
            Debug.WriteLine("DDSFile: Height = " + header.DwHeight);
            Debug.WriteLine("DDSFile: MipMapCount = " + header.DwMipMapCount);
            Debug.WriteLine("DDSFile: PitchOrLinearSize = " + header.DwPitchOrLinearSize);
            Debug.WriteLine("DDSFile: Depth = " + header.DwDepth);
            Debug.WriteLine("DDSFile: DwReserved1 = " + header.DwReserved1);
            Debug.WriteLine("DDSFile: DwReserved2 = " + header.DwReserved2);
            Debug.WriteLine("DDSFile: DwCaps = " + header.DwCaps);
            Debug.WriteLine("DDSFile: DwCaps2 = " + header.DwCaps2);
            Debug.WriteLine("DDSFile: DwCaps3 = " + header.DwCaps3);
            Debug.WriteLine("DDSFile: DwCaps4 = " + header.DwCaps4);
            Debug.WriteLine("DDSFile: DwFlags = " + header.DwFlags);

            Debug.WriteLine("DDSFile: PF: DwFourCC = " + header.Ddspf.DwFourCC);
            Debug.WriteLine("DDSFile: PF: DwFlags = " + header.Ddspf.DwFlags);
            Debug.WriteLine("DDSFile: PF: DwRGBBitCount = " + header.Ddspf.DwRGBBitCount);
            Debug.WriteLine("DDSFile: PF: DwRBitMask = " + header.Ddspf.DwRBitMask);
            Debug.WriteLine("DDSFile: PF: DwGBitMask = " + header.Ddspf.DwGBitMask);
            Debug.WriteLine("DDSFile: PF: DwBBitMask = " + header.Ddspf.DwBBitMask);
            Debug.WriteLine("DDSFile: PF: DwABitMask = " + header.Ddspf.DwABitMask);

            if ((header.Ddspf.DwFlags & (uint)0x4) != 0)
            {
                Debug.WriteLine("DDSFile: DDPF_FOURCC");
            }

            if ((header.Ddspf.DwFlags & (uint)0x40) != 0)
            {
                Debug.WriteLine("DDSFile: file is uncompressed");
            }

            if (isDX10)
            {
                Debug.WriteLine("DDSFile: DX10 DXGI Format = " + dx10header.DxgiFormat);
                Debug.WriteLine("DDSFile: DX10 Resource Dimension = " + dx10header.ResourceDimension);
                Debug.WriteLine("DDSFile: DX10 Misc Flag = " + dx10header.MiscFlag);
                Debug.WriteLine("DDSFile: DX10 Array Size = " + dx10header.ArraySize);
                Debug.WriteLine("DDSFile: DX10 Misc Flags 2 = " + dx10header.MiscFlags2);
            }

        }

        public DdsHeader ReadDDSHeader(Stream fileStream)
        {
            BinaryReader reader = new BinaryReader(fileStream);
            uint DwMagic = reader.ReadUInt32();
            if (DwMagic != 0x20534444)
            {
                throw new Exception("Invalid DDS file, bad magic");
            }


            DdsHeader header = new DdsHeader();
            header.DwSize = reader.ReadUInt32();
            header.DwFlags = reader.ReadUInt32();
            header.DwHeight = reader.ReadUInt32();
            header.DwWidth = reader.ReadUInt32();
            header.DwPitchOrLinearSize = reader.ReadUInt32();
            header.DwDepth = reader.ReadUInt32();
            header.DwMipMapCount = reader.ReadUInt32();
            header.DwReserved1 = new uint[11];
            for (int i = 0; i < 11; i++)
            {
                header.DwReserved1[i] = reader.ReadUInt32();
            }
            header.Ddspf = new DdsPixelFormat();
            header.Ddspf.DwSize = reader.ReadUInt32();
            header.Ddspf.DwFlags = reader.ReadUInt32();
            header.Ddspf.DwFourCC = reader.ReadUInt32();
            header.Ddspf.DwRGBBitCount = reader.ReadUInt32();
            header.Ddspf.DwRBitMask = reader.ReadUInt32();
            header.Ddspf.DwGBitMask = reader.ReadUInt32();
            header.Ddspf.DwBBitMask = reader.ReadUInt32();
            header.Ddspf.DwABitMask = reader.ReadUInt32();
            header.DwCaps = reader.ReadUInt32();
            header.DwCaps2 = reader.ReadUInt32();
            header.DwCaps3 = reader.ReadUInt32();
            header.DwCaps4 = reader.ReadUInt32();
            header.DwReserved2 = reader.ReadUInt32();

            return header;
        }

        public DdsHeaderDX10 ReadDX10Header(Stream fileStream)
        {
            BinaryReader reader = new BinaryReader(fileStream);
            DdsHeaderDX10 header = new DdsHeaderDX10();
            header.DxgiFormat = reader.ReadUInt32();
            header.ResourceDimension = reader.ReadUInt32();
            header.MiscFlag = reader.ReadUInt32();
            header.ArraySize = reader.ReadUInt32();
            header.MiscFlags2 = reader.ReadUInt32();

            return header;
        }

    }
}
