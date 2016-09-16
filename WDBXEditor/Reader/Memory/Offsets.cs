using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDBXEditor.Reader.Memory
{
    static class Offsets
    {
        /// <summary>
        /// 1.12.1 5875
        /// </summary>
        public static OffsetMap Classic => new OffsetMap()
        {
            ClientConnection = 0x00B41414,
            ObjectManager = 0x0, //Unused
            FirstObjectOffset = 0xAC,
            LocalGuidOffset = 0xC0,
            NextObjectOffset = 0x3C,
            LocalPlayerGUID = 0x87BD74,
            MapID = 0x00468580,
            Guid = 0x30,
            Pos_X = 0x09B8,
            Pos_Y = 0x09BC,
            Pos_Z = 0x09C0,
            Rot = 0x09C4,
        };

        /// <summary>
        /// 2.4.3
        /// </summary>
        public static OffsetMap TBC => new OffsetMap()
        {
            ClientConnection = 0x00D43318,
            ObjectManager = 0x2218,
            FirstObjectOffset = 0xAC,
            LocalGuidOffset = 0xC0,
            NextObjectOffset = 0x3C,
            LocalPlayerGUID = 0x943340,
            MapID = 0x7A9AC8, //?
            Guid = 0x30,
            Pos_X = 0xBF0,
            Pos_Y = 0xBF4,
            Pos_Z = 0xBF8,
            Rot = 0xBFC,
        };

        /// <summary>
        /// 3.3.5a 
        /// </summary>
        public static OffsetMap WotLK => new OffsetMap()
        {
            ClientConnection = 0x00C79CE0,
            ObjectManager = 0x2ED0,
            FirstObjectOffset = 0xAC,
            LocalGuidOffset = 0xC0,
            NextObjectOffset = 0x3C,
            LocalPlayerGUID = 0xBD07A8,
            MapID = 0x00AB63BC,
            Guid = 0x30,
            Pos_X = 0x798,
            Pos_Y = 0x79C,
            Pos_Z = 0x7A0,
            Rot = 0x7A8,
        };

        /// <summary>
        /// 4.3.4
        /// </summary>
        public static OffsetMap Cata => new OffsetMap()
        {
            ClientConnection = 0x9BE7E0,
            ObjectManager = 0x463C,
            FirstObjectOffset = 0xC0,
            LocalGuidOffset = 0x30,
            NextObjectOffset = 0x3C,
            LocalPlayerGUID = 0x9BE818,
            MapID = 0x8A2710,
            Guid = 0x30,
            Pos_X = 0x790,
            Pos_Y = 0x794,
            Pos_Z = 0x798,
            Rot = 0x7A0,
        };

        /// <summary>
        /// 5.4.8 x86
        /// </summary>
        public static OffsetMap Mopx86 => new OffsetMap()
        {
            ClientConnection = 0xCB47C4,
            ObjectManager = 0x0, //??
            FirstObjectOffset = 0xC0,
            LocalGuidOffset = 0x30,
            NextObjectOffset = 0x3C,
            LocalPlayerGUID = 0x9BE818,
            MapID = 0x8A2710,
            Guid = 0x30,
            Pos_X = 0x838,
            Pos_Y = 0x83C,
            Pos_Z = 0x840,
            Rot = 0x844,
        };
    }

    public class OffsetMap
    {
        public ulong ClientConnection { get; set; }
        public ulong ObjectManager { get; set; }
        public ulong FirstObjectOffset { get; set; }
        public ulong LocalGuidOffset { get; set; }
        public ulong NextObjectOffset { get; set; }
        public ulong LocalPlayerGUID { get; set; }
        public ulong MapID { get; set; }
        public ulong Pos_X { get; set; }
        public ulong Pos_Y { get; set; }
        public ulong Pos_Z { get; set; }
        public ulong Rot { get; set; }
        public ulong Guid { get; set; }
    }
}
