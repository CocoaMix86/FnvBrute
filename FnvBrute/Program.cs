using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace FnvBrute
{
    class Program
    {
        private static Stopwatch _stopwatch = new Stopwatch();

        static void Main(string[] args)
        {
            args = new string[] { "a07e101f", "2", "7"};
            uint match = 0;
            int maxLength = 0;
            int minLength = 0;

            if (args.Length == 3) {
                // Parse hash param
                try {
                    match = Convert.ToUInt32(args[0], 16);
                } catch (Exception e) {
                    Console.WriteLine("Error reading first argument (hash): " + e.Message);
                }

                // Parse length param
                try {
                    maxLength = int.Parse(args[2]);
                    minLength = int.Parse(args[1]);
                } catch (Exception e) {
                    Console.WriteLine("Error reading second argument (maxLength): " + e.Message);
                }
            }

            if (args.Length < 3 || match == 0 || minLength < 2 || maxLength < 2) {
                Console.WriteLine("\nFnvBrute FNV-1 (32-bit) hash collider, https://github.com/CocoaMix86/FnvBrute");
                Console.WriteLine("Usage: FnvBrute.exe [hash] [minLength] [maxLength]");
                Console.WriteLine("Hash should be a uint32 in hexadecimal form (8 characters)");
                Console.WriteLine("minLength and maxLength should be equal or greater than 2");
                Console.WriteLine("Example: FnvBrute.exe 0x1234abcd 5 12\n");
                return;
            }

            Console.WriteLine($"Hash: {match}, max plaintext length: {maxLength}");
            _stopwatch.Start();

            for (int length = minLength; length <= maxLength; length++)
            {
                CreateFnvThread(length, match);
            }

            var exitEvent = new ManualResetEventSlim();
            exitEvent.Wait();
        }

        static void CreateFnvThread(int length, uint match)
        {
            var thread = new Thread(() =>
            {
                Console.WriteLine("Creating hasher for length " + length);
                //StartFnv(length, match);
                Bruteforce(length, match);
                Console.WriteLine($"Hasher for length {length} finished in {_stopwatch.ElapsedMilliseconds / 1000}s");
            });

            thread.Start();
        }


        //private List<uint> tofind = new List<uint>() { 0x8b94465f, 0x1fe1cdd8, 0x2c16d809, 0x58c0f90f, 0x1814bfd4, 0xd61447d3, 0x8c8b5767, 0x8c17f344, 0xaffd1047, 0x6d90b646, 0x134d04b5, 0x74698ce8, 0x75df8462, 0xedc78320, 0x33593638, 0xc08dc572, 0x6efd7dc5, 0x75df8462, 0x05099ded, 0xf646d5e7, 0x51b7d0e3, 0xc2cf9f73, 0x9f75fd34, 0x2ab1a6a9, 0x4bf4650d, 0xac75d66e, 0x8ab4684d, 0xce326723, 0x6f741307, 0x5472d655, 0x1b6b5506, 0x3347011c, 0xd1b89548, 0x40a89b31, 0x54ef8e79, 0xc8266ef9, 0x7ba5c8e0, 0x0810c3b9, 0x13bdaa3b, 0xd7b602ce, 0x13bdaa3b, 0x283c6e78, 0x9b4504d6, 0x887ca5dd };
        private uint[] tofind = new uint[] { 0xd61447d3, 0x8c8b5767, 0x8c17f344 };

        static void Bruteforce(int length, uint match)
        {
            byte[] chars = Encoding.UTF8.GetBytes("abcdefghijklmnopqrstuvwxyz");
            //char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            Parallel.ForEach(chars, index => {
                byte[] _bytes = new byte[length];
                _bytes[0] = index;
                for (var i = 1; i < _bytes.Length; i++) {
                    // set up every other byte to chain Increment() for the whole array
                    _bytes[i] = (byte)'a';
                }

                while (true) {
                    var depth = _bytes.Length - 1;
                    while (_bytes.Increment(depth)) {
                        depth--;
                        if (depth == 0)
                            goto stop;// all permutations at this length are done
                    }
                    uint hash = Hash32(_bytes);
                    //if (hash == match)
                        //Console.WriteLine($"Length {length} match: >> {hash.ToString("x8")} - {Encoding.ASCII.GetString(_bytes)} << in {_stopwatch.ElapsedMilliseconds / 1000}s");
                }
            stop:;
            });
        }

        private const uint OffsetBasis = 2166136261;
        private const uint Prime = 16777619;
        static uint Hash32(byte[] s)
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
    }

    static class Utilities
    {
        // returns: whether the specified byte has completed a loop
        public static bool Increment(this byte[] array, int i)
        {
            array[i]++;

            switch (array[i]) {
                case (byte)'{':
                    array[i] = (byte)'0';
                    return false;
                case (byte)':':
                    array[i] = (byte)'.';
                    return false;
                case (byte)'/':
                    array[i] = (byte)'_';
                    return false;
                case (byte)'`':
                    array[i] = (byte)'a';
                    return true;
                default:
                    return false;
                    
            }
        }
    }
}
