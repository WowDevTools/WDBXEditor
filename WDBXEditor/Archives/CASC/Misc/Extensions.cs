using System;
using System.IO;

namespace WDBXEditor.Archives.Misc
{
    public static class Extensions
    {
        public static byte[] ToByteArray(this string s)
        {
            var data = new byte[s.Length / 2];

            for (int i = 0; i < s.Length; i += 2)
                data[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);

            return data;
        }

        public static string GetHexAt(this string value, int index)
        {
            var hex = Convert.ToByte(value.Substring(index, 2), 16);

            return $"{hex:x2}";
        }

        public static bool Compare(this byte[] b, byte[] b2)
        {
            for (int i = 0; i < b2.Length; i++)
                if (b[i] != b2[i])
                    return false;

            return true;
        }

        public static T[] Slice<T>(this T[] arr, int start, int end)
        {
            var newLength = end - start;
            var ret = new T[newLength];

            for (var i = 0; i < newLength; i++)
                ret[i] = arr[start + i];

            return ret;
        }

        public static ushort ReadBEUInt16(this BinaryReader br)
        {
            return (ushort)System.Net.IPAddress.HostToNetworkOrder(br.ReadInt16());
        }

        public static uint ReadBEUInt32(this BinaryReader br)
        {
            return (uint)System.Net.IPAddress.HostToNetworkOrder(br.ReadInt32());
        }

        public static uint ReadUInt24(this BinaryReader br)
        {
            var bytes = br.ReadBytes(3);

            return (uint)((bytes[0] << 16) | (bytes[1] << 8) | bytes[2]);
        }
    }
}
