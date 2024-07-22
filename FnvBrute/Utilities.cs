namespace FnvBrute
{
    static class Utilities
    {
        // returns: whether the specified byte has completed a loop
        public static bool Increment(this byte[] array, int i)
        {
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
            return false;
        }

        public static bool Increment(this byte from, out byte result)
        {
            result = (byte)(from + 1);
            if (result == 0x3a)
            {
                // digits out of bound, continue to alphabet
                result = 0x61;
                return false;
            }
            else if (result == 0x7b)
            {
                // alphabet out of bound, continue to underscore
                result = 0x5f;
                return false;
            }
            else if (result == 0x60)
            {
                // uppercase out of bound, all done
                result = 0x30;
                return true;
            }

            // keep incrementing
            return false;
        }

        public static void ZeroFrom(this uint[] array, int i)
        {
            while (i < array.Length)
            {
                array[i] = 0;
                i++;
            }
        }
    }
}
