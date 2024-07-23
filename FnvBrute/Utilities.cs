using System.Collections.Generic;
using System.Linq;

namespace FnvBrute
{
    static class Utilities
    {
        public static List<char> chars = "-abcdefghijklmnopqrstuvwxyz0123456789._".ToCharArray().ToList();
        //public static List<char> chars = "-abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().ToList();
        // returns: whether the specified byte has completed a loop
        public static bool Increment(this byte[] array, int i)
        {
            /*
            var result = (byte)(array[i] + 1);

            if (result == '`') {
                array[i] = (byte)'.';
                return false;
            }
            // digits out of bound, continue to alphabet
            // : comes after 9
            else if (result == ':')
            {
                array[i] = (byte)'a';
                return false;
            }
            // alphabet out of bound
            // } comes after z
            else if (result == '{')
            {
                return true;
            }

            // keep incrementing
            array[i] = result;
            */
            if (chars.IndexOf((char)array[i]) + 1 > chars.Count - 1) {
                array[i] = (byte)chars[1];
                return true;
            }
            else {
                array[i] = (byte)chars[chars.IndexOf((char)array[i]) + 1];
                return false;
            }
        }
    }
}
