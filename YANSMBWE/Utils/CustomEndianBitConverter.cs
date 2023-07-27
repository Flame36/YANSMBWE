using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Buffers.Binary;

namespace YANSMBWE.Utils
{
    public static class CustomEndianBitConverter
    {

        public static uint ToUInt32(byte[] data, int index = 0, bool LittleEndian = false)
        {
            UInt32 res = BitConverter.ToUInt32(data.SubArray(index, 4));
            if (BitConverter.IsLittleEndian == LittleEndian)
                return res;
            else
                return BinaryPrimitives.ReverseEndianness(res);
        }

    }
}
