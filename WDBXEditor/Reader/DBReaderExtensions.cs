﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WDBXEditor.Reader
{
    public static class DBReaderExtensions
    {
        public static string ReadStringNull(this BinaryReader reader)
        {
            byte num;
            List<byte> temp = new List<byte>();

            while ((num = reader.ReadByte()) != 0)
                temp.Add(num);

            return Encoding.UTF8.GetString(temp.ToArray());
        }

        public static sbyte[] ReadSByte(this BinaryReader br, int count)
        {
            var arr = new sbyte[count];
            for (int i = 0; i < count; i++)
                arr[i] = br.ReadSByte();

            return arr;
        }

        public static byte[] ReadByte(this BinaryReader br, int count)
        {
            var arr = new byte[count];
            for (int i = 0; i < count; i++)
                arr[i] = br.ReadByte();

            return arr;
        }

        public static int[] ReadInt32(this BinaryReader br, int count)
        {
            var arr = new int[count];
            for (int i = 0; i < count; i++)
                arr[i] = br.ReadInt32();

            return arr;
        }

        public static uint[] ReadUInt32(this BinaryReader br, int count)
        {
            var arr = new uint[count];
            for (int i = 0; i < count; i++)
                arr[i] = br.ReadUInt32();

            return arr;
        }

        public static float[] ReadSingle(this BinaryReader br, int count)
        {
            var arr = new float[count];
            for (int i = 0; i < count; i++)
                arr[i] = br.ReadSingle();

            return arr;
        }

        public static long[] ReadInt64(this BinaryReader br, int count)
        {
            var arr = new long[count];
            for (int i = 0; i < count; i++)
                arr[i] = br.ReadInt64();

            return arr;
        }

        public static ulong[] ReadUInt64(this BinaryReader br, int count)
        {
            var arr = new ulong[count];
            for (int i = 0; i < count; i++)
                arr[i] = br.ReadUInt64();

            return arr;
        }

        public static string ReadString(this BinaryReader br, int count)
        {
            byte[] stringArray = br.ReadBytes(count);
            return Encoding.UTF8.GetString(stringArray);
        }

        public static int ReadInt32(this BinaryReader br, FieldStructureEntry map = null)
        {
            if (map == null)
                return br.ReadInt32();

            byte[] b = br.ReadBytes(map.ByteCount);
            int i32 = 0;
            for (int i = 0; i < b.Length; i++)
                i32 |= (b[i] << i * 8);

            return i32;
        }

        public static uint ReadUInt32(this BinaryReader br, FieldStructureEntry map = null)
        {
            if (map == null)
                return br.ReadUInt32();

            byte[] b = br.ReadBytes(map.ByteCount);
            uint u32 = 0;
            for (int i = 0; i < b.Length; i++)
                u32 |= ((uint)b[i] << i * 8);

            return u32;
        }

        public static long ReadInt64(this BinaryReader br, FieldStructureEntry map = null)
        {
            if (map == null)
                return br.ReadInt64();

            byte[] b = br.ReadBytes(map.ByteCount);
            long i64 = 0;
            for (int i = 0; i < b.Length; i++)
                i64 |= ((long)b[i] << i * 8);

            return i64;
        }

        public static ulong ReadUInt64(this BinaryReader br, FieldStructureEntry map = null)
        {
            if (map == null)
                return br.ReadUInt64();

            byte[] b = br.ReadBytes(map.ByteCount);
            ulong u64 = 0;
            for (int i = 0; i < b.Length; i++)
                u64 |= ((ulong)b[i] << i * 8);

            return u64;
        }

        public static void Scrub(this BinaryReader br, long pos)
        {
            br.BaseStream.Position = pos;
        }

        public static void Scrub(this BinaryWriter br, long pos)
        {
            br.BaseStream.Position = pos;
        }


        public static void WriteInt32(this BinaryWriter bw, int value, FieldStructureEntry map = null)
        {
            if (map == null)
                bw.Write(value);
            else
                bw.Write(BitConverter.GetBytes(value), 0, map.ByteCount);
        }

        public static void WriteUInt32(this BinaryWriter bw, uint value, FieldStructureEntry map = null)
        {
            if (map == null)
                bw.Write(value);
            else
                bw.Write(BitConverter.GetBytes(value), 0, map.ByteCount);
        }

        public static void WriteInt64(this BinaryWriter bw, long value, FieldStructureEntry map = null)
        {
            if (map == null)
                bw.Write(value);
            else
                bw.Write(BitConverter.GetBytes(value), 0, map.ByteCount);
        }

        public static void WriteUInt64(this BinaryWriter bw, ulong value, FieldStructureEntry map = null)
        {
            if (map == null)
                bw.Write(value);
            else
                bw.Write(BitConverter.GetBytes(value), 0, map.ByteCount);
        }

        public static void WriteArray<T>(this BinaryWriter bw, T[] data)
        {
            byte[] bytes = new byte[data.Length * System.Runtime.InteropServices.Marshal.SizeOf(typeof(T))];
            Buffer.BlockCopy(data, 0, bytes, 0, bytes.Length);
            bw.Write(bytes);
        }
    }
}