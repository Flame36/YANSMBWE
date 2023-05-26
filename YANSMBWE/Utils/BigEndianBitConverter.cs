using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YANSMBWE.Utils
{
    public static class BigEndianBitConverter
    {

        public static uint ToUInt32(byte[] data, int index = 0)
        {
            if (!BitConverter.IsLittleEndian)
                return BitConverter.ToUInt32(data.SubArray(index, 4));

            byte[] buffer = data.SubArray(index, 4);
            Array.Reverse(buffer);
            return BitConverter.ToUInt32(buffer);
        }

    }
}
