using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YANSMBWE.Utils
{
    public static class Extensions
    {
        //This should really not be done
        //TODO: Implement better way
        public static T[] SubArray<T>(this T[] data, uint index, uint length)
            => data.SubArray((int)index, (int)length);
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
    }
}
