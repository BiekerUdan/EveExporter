﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveExporter.GrannyNative
{
    internal partial class Granny
    {
        // The first 16 bytes of the gr2 file is a "format" specifier
        byte[] LittleEndian32v1 = { 0x29, 0xDE, 0x6C, 0xC0, 0xBA, 0xA4, 0x53, 0x2B, 0x25, 0xF5, 0xB7, 0xA5, 0xF6, 0x66, 0xE2, 0xEE };
        byte[] LittleEndian32v2 = { 0x29, 0x75, 0x31, 0x82, 0xBA, 0x02, 0x11, 0x77, 0x25, 0x3A, 0x60, 0x2F, 0xF6, 0x6A, 0x8C, 0x2E };
        byte[] LittleEndian32v3 = { 0xB8, 0x67, 0xB0, 0xCA, 0xF8, 0x6D, 0xB1, 0x0F, 0x84, 0x72, 0x8C, 0x7E, 0x5E, 0x19, 0x00, 0x1E };
        byte[] LittleEndian32v4 = { 0x5B, 0x6C, 0xD6, 0xD2, 0x3C, 0x46, 0x8B, 0xD6, 0x83, 0xC2, 0xAA, 0x99, 0x3F, 0xE1, 0x76, 0x52 };
        byte[] LittleEndian64v1 = { 0xE5, 0x9B, 0x49, 0x5E, 0x6F, 0x63, 0x1F, 0x14, 0x1E, 0x13, 0xEB, 0xA9, 0x90, 0xBE, 0xED, 0xC4 };
        byte[] LittleEndian64v2 = { 0xE5, 0x2F, 0x4A, 0xE1, 0x6F, 0xC2, 0x8A, 0xEE, 0x1E, 0xD2, 0xB4, 0x4C, 0x90, 0xD7, 0x55, 0xAF };
        byte[] BigEndian32v1 = {0x0E, 0x11, 0x95, 0xB5, 0x6A, 0xA5, 0xB5, 0x4B, 0xEB, 0x28, 0x28, 0x50, 0x25, 0x78, 0xB3, 0x04 };
        byte[] BigEndian32v2 = {0x0E, 0x74, 0xA2, 0x0A, 0x6A, 0xEB, 0xEB, 0x64, 0xEB, 0x4E, 0x1E, 0xAB, 0x25, 0x91, 0xDB, 0x8F };
        byte[] BigEndian32v3 = {0xB5, 0x95, 0x11, 0x0E, 0x4B, 0xB5, 0xA5, 0x6A, 0x50, 0x28, 0x28, 0xEB, 0x04, 0xB3, 0x78, 0x25 };
        byte[] BigEndian64v1 = { 0x31, 0x95, 0xD4, 0xE3, 0x20, 0xDC, 0x4F, 0x62, 0xCC, 0x36, 0xD0, 0x3A, 0xB1, 0x82, 0xFF, 0x89 };
        byte[] BigEndian64v2 = { 0x31, 0xC2, 0x4E, 0x7C, 0x20, 0x40, 0xA3, 0x25, 0xCC, 0xE1, 0xC2, 0x7A, 0xB1, 0x32, 0x49, 0xF3 };

    }
}