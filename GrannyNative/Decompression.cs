using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
//using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.DataFormats;

namespace EveExporter.GrannyNative
{
    internal class Decompression
    {
        private const string DLL_PATH_X64 = "granny2_x64.dll";

        [DllImport(DLL_PATH_X64)]
        private static extern int GrannyDecompressData(int a, int b, int c, IntPtr d, int e, int f, int g, IntPtr h);

        [DllImport(DLL_PATH_X64)]
        private static extern IntPtr GrannyBeginFileDecompression(int Format, int Endianes, int DecompressedSize, IntPtr DecompressionBuffer, int WorkBufferSize, IntPtr WorkBuffer);

        [DllImport(DLL_PATH_X64)]
        private static extern int GrannyDecompressIncremental(IntPtr a, int b, IntPtr c);

        [DllImport(DLL_PATH_X64)]
        private static extern int GrannyEndFileDecompression(IntPtr a);



        public void GR2decompress(byte[] DecompressedData, byte[] CompressedData, uint DecompressedSize, uint CompressedSize, SectionHeader Section)
        {
            int reverseBytes = 0;  // 0 = little endian, 1 = big endian but we don't support big yet
            IntPtr decompression_handle;

            GCHandle DecompressedDataPinnedArray = GCHandle.Alloc(DecompressedData, GCHandleType.Pinned);
            IntPtr DecompressedDataPointer = DecompressedDataPinnedArray.AddrOfPinnedObject();

            GCHandle CompressedDataPinnedArray = GCHandle.Alloc(CompressedData, GCHandleType.Pinned);
            IntPtr CompressedDataPointer = CompressedDataPinnedArray.AddrOfPinnedObject();

            Debug.WriteLine("Decompressing section of size " + CompressedSize + " to " + DecompressedSize);

            if (Section.Compression == 1 || Section.Compression == 2)
            {
                var result = GrannyDecompressData((int)Section.Compression, reverseBytes, (int)CompressedSize, CompressedDataPointer, (int)Section.First16Bit, (int)Section.First8Bit, (int)DecompressedSize, DecompressedDataPointer);
            }

            unsafe
            {
                if (Section.Compression == 3 || Section.Compression == 4)
                {
                    int WorkSizeMem = 0x10000;
                    IntPtr WorkMemBuffer = Marshal.AllocHGlobal(WorkSizeMem);

                    decompression_handle = GrannyBeginFileDecompression((int)Section.Compression, 0, (int)DecompressedSize, DecompressedDataPointer, WorkSizeMem, WorkMemBuffer);

                    int Position = 0;
                    byte* bytePointer = (byte*)CompressedDataPointer;

                    while (Position < CompressedSize)
                    {
                        int chunkSize = Math.Min((int)CompressedSize - Position, 0x2000);
                        byte* offsetPointer = bytePointer + Position;

                        int chunk_result = GrannyDecompressIncremental(decompression_handle, chunkSize, new IntPtr(offsetPointer));

                        if (chunk_result != 1)
                        {
                            throw new Exception("Decompression failed");
                        }

                        Debug.WriteLine("Decompressed chunk of size " + chunkSize + " at offset " + Position);

                        Position += chunkSize;
                    }

                    int result = GrannyEndFileDecompression(decompression_handle);

                    if (result != 1)
                    {
                        throw new Exception("Decompression failed");
                    }
                }
            }

            DecompressedDataPinnedArray.Free();
            CompressedDataPinnedArray.Free();

        }
    }
}
