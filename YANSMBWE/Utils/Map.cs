using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YANSMBWE.Utils
{
    //TODO: Refactor this
    public class Map<T1, T2>
        where T1 : notnull
        where T2 : notnull
    {
        readonly Dictionary<T1, T2> forward = new();
        readonly Dictionary<T2, T1> reverse = new();

        public T1 this[T2 index]
        {
            get { return reverse[index]; }
        }
        public T2 this[T1 index]
        {
            get { return forward[index]; }
        }

        public bool Add(T1 value1, T2 value2)
        {
            if (!(forward.ContainsKey(value1) && reverse.ContainsKey(value2)))
                return false;

            forward.Add(value1, value2);
            reverse.Add(value2, value1);
            return true;
        }
        private bool Remove(T1 value1, T2 value2)
        {
            if (!(
                forward.ContainsKey(value1) &&
                reverse.ContainsKey(value2)
                ))
                return false;

            forward.Remove(value1);
            reverse.Remove(value2);
            return true;
        }
        public bool Remove(T1 value)
        {
            if (!(forward.ContainsKey(value) && reverse.ContainsValue(value)))
                return false;
            return Remove(value, forward[value]);
        }
        public bool Remove(T2 value)
        {
            if (!(forward.ContainsValue(value) && reverse.ContainsKey(value)))
                return false;
            return Remove(reverse[value], value);
        }
    }
}
