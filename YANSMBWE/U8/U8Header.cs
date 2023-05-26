using HarfBuzzSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YANSMBWE.Utils;

namespace YANSMBWE.U8
{
    public class U8Header
    {
        public static byte[] Magic { get; } = new byte[] { 0x55, 0xAA, 0x38, 0x2D };
        public byte[] Tag { get; set; }
        public UInt32 RootNodeOffset { get; set; }
        public UInt32 HeaderSize { get; set; }
        public UInt32 DataOffset { get; set; }

        public U8Header(byte[] Tag, UInt32 RootNodeOffset, UInt32 HeaderSize, UInt32 DataOffset)
        {
            this.Tag = Tag;
            this.RootNodeOffset = RootNodeOffset;
            this.HeaderSize = HeaderSize;
            this.DataOffset = DataOffset;
        }

        public static U8Header FromBytes(byte[] data)
        {
            byte[] tag = data.SubArray(0, 4);
            UInt32 rootNodeOffset = BigEndianBitConverter.ToUInt32(data, 4);
            UInt32 headerSize = BigEndianBitConverter.ToUInt32(data, 8);
            UInt32 dataOffset = BigEndianBitConverter.ToUInt32(data, 12);

            return new U8Header(tag, rootNodeOffset, headerSize, dataOffset);
        }
    }
}
