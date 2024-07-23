using System;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Combinatorics;
using Combinatorics.Collections;

namespace FnvBrute
{
    class FnvHasher
    {
        private const uint OffsetBasis = 2166136261;
        private const uint Prime = 16777619;

        // digits are 0x30 to 0x39
        // lowercase alphabet is 0x61 to 0x7a
        // underscore is 0x5f

        //private List<uint> tofind = new List<uint>() { 0x8b94465f, 0x1fe1cdd8, 0x2c16d809, 0x58c0f90f, 0x1814bfd4, 0xd61447d3, 0x8c8b5767, 0x8c17f344, 0xaffd1047, 0x6d90b646, 0x134d04b5, 0x74698ce8, 0x75df8462, 0xedc78320, 0x33593638, 0xc08dc572, 0x6efd7dc5, 0x75df8462, 0x05099ded, 0xf646d5e7, 0x51b7d0e3, 0xc2cf9f73, 0x9f75fd34, 0x2ab1a6a9, 0x4bf4650d, 0xac75d66e, 0x8ab4684d, 0xce326723, 0x6f741307, 0x5472d655, 0x1b6b5506, 0x3347011c, 0xd1b89548, 0x40a89b31, 0x54ef8e79, 0xc8266ef9, 0x7ba5c8e0, 0x0810c3b9, 0x13bdaa3b, 0xd7b602ce, 0x13bdaa3b, 0x283c6e78, 0x9b4504d6, 0x887ca5dd };
        private List<uint> tofind = new List<uint>() { 0xd61447d3, 0x8c8b5767, 0x8c17f344 };

        //private HashSet<string> skipthese = new HashSet<string>() { "...", "aaa", "bbb", "ccc", "ddd", "eee", "fff", "ggg", "hhh", "iii", "jjj", "kkk", "lll", "mmm", "nnn", "ooo", "ppp", "qqq", "rrr", "sss", "ttt", "uuu", "vvv", "www", "xxx", "yyy", "zzz" };
        private HashSet<string> skipthese = new HashSet<string>() { "..." };

        public void Bruteforce(int length, uint match, OnMatchFound callback)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            byte[] combochars = "abcdefghijklmnopqrstuvwxyz0123456789._".ToCharArray().Select(c => (byte)c).ToArray();
            Variations<byte> perms = new Variations<byte>(combochars, length, GenerateOption.WithRepetition);

            Parallel.ForEach(perms, _bytes => {  
                uint hash = Hash32(_bytes);
                if (hash == match) {
                    callback(length, $"{hash.ToString("x8")} - {Encoding.ASCII.GetString(_bytes.ToArray())}");
                }
            });

            /*
            Parallel.ForEach(chars, index => {
                byte[] _bytes = new byte[length];
                _bytes[0] = (byte)index;
                for (var i = 1; i < _bytes.Length; i++) {
                    // set up every other byte to chain Increment() for the whole array
                    _bytes[i] = (byte)'a';
                }
                _bytes[length - 1] = (byte)'-';

                //Initialize(_bytes, length, chars[index]);

                while (true) {
                    var depth = _bytes.Length - 1;
                    while (_bytes.Increment(depth)) {
                        depth--;
                        if (depth == 0)
                            goto stop;// all permutations at this length are done
                    }
                    /*
                    if (skipthese.Any(x => Encoding.ASCII.GetString(_bytes).Contains(x)))
                        goto skip;
                    
                    uint hash = Hash32(_bytes);
                    if (hash == match) {
                        callback(length, $"{hash.ToString("x8")} - {Encoding.ASCII.GetString(_bytes)}");
                    }
                skip:;
                }
            stop:;
            });
    */
        }

        public uint Hash32(byte[] s)
        {
            uint h = OffsetBasis;
            foreach (byte c in s)
                h = (h ^ c) * Prime;
            h *= 0x2001;
            h ^= h >> 0x7;
            h *= 0x9;
            h ^= h >> 0x11;
            h *= 0x21;

            return h;
        }
        public uint Hash32(IReadOnlyList<byte> s)
        {
            uint h = OffsetBasis;
            foreach (byte c in s)
                h = (h ^ c) * Prime;
            h *= 0x2001;
            h ^= h >> 0x7;
            h *= 0x9;
            h ^= h >> 0x11;
            h *= 0x21;

            return h;
        }

        public delegate void OnMatchFound(int length, string match);
    }
}
