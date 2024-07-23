﻿using System.Collections.Generic;
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
            byte result = (byte)(array[i] + 1);

            if (result == '{') {
                array[i] = (byte)'0';
                return false;
            }
            if (result == ':') {
                array[i] = (byte)'.';
                return false;
            }
            if (result == '/') {
                array[i] = (byte)'_';
                return false;
            }
            if (result == '`') {
                array[i] = (byte)'a';
                return true;
            }

            array[i]++;
            return false;
            
        }
    }
}
