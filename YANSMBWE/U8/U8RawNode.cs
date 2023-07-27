using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YANSMBWE.Utils;

namespace YANSMBWE.U8
{
    public class U8RawNode
    {
        public byte Type { get; set; }
        public uint NameOffset { get; set; } //Uint24
        public uint DataOffset { get; set; }
        public uint Size { get; set; }

        public int NodeIndex { get; set; }

        public U8RawNode(byte Type, uint NameOffset, uint DataOffset, uint Size, int NodeIndex)
        {
            this.Type = Type;
            this.NameOffset = NameOffset;
            this.DataOffset = DataOffset;
            this.Size = Size;
            this.NodeIndex = NodeIndex;
        }

        public static U8RawNode FromBytes(byte[] data, U8Header header, int nodeIndex = 0)
        {
            int index = (int)(header.RootNodeOffset + nodeIndex * U8Archive.NODE_LENGTH);
            byte type = data[index];
            //Sketchy, but is essentially the same as reading a 24-bit uint
            uint nameOffset = CustomEndianBitConverter.ToUInt32(data, index) % 16777216;
            uint dataOffset = CustomEndianBitConverter.ToUInt32(data, index + 4);
            uint size = CustomEndianBitConverter.ToUInt32(data, index + 8);

            return new U8RawNode(type, nameOffset, dataOffset, size, nodeIndex);
        }
    }
}
