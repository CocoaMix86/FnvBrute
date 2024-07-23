using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FnvBrute
{
    class Program
    {
        private static Stopwatch _stopwatch = new Stopwatch();
        static void Main(string[] args)
        {
            args = new string[] { "a07e101f", "2", "7"};
            uint match = Convert.ToUInt32(args[0], 16);
            int maxLength = int.Parse(args[2]);
            int minLength = int.Parse(args[1]);
            _stopwatch.Start();
            for (int length = minLength; length <= maxLength; length++) {
                CreateFnvThread(length, match);
            }
            var exitEvent = new ManualResetEventSlim();
            exitEvent.Wait();
        }

        static void CreateFnvThread(int length, uint match)
        {
            var thread = new Thread(() => {
                Console.WriteLine("Creating hasher for length " + length);
                //StartFnv(length, match);
                Bruteforce(length, match);
                Console.WriteLine($"Hasher for length {length} finished in {_stopwatch.ElapsedMilliseconds / 1000}s");
            });
            thread.Start();
        }

        static void Bruteforce(int length, uint match)
        {
            byte[] chars = Encoding.UTF8.GetBytes("abcdefghijklmnopqrstuvwxyz");
            Parallel.ForEach(chars, () => match, (index, loop, match2) => {
                byte[] _bytes = new byte[length];
                _bytes[0] = index;
                for (var i = 1; i < _bytes.Length; i++) {
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
                    if (hash == match2)
                        Console.WriteLine($"Length {length} match: >> {hash.ToString("x8")} - {Encoding.ASCII.GetString(_bytes)} << in {_stopwatch.ElapsedMilliseconds / 1000}s");
                }
            stop:;
                return match2;
            }, (match2) => { });
        }

        private const uint OffsetBasis = 2166136261;
        private const uint Prime = 16777619;
        static uint Hash32(byte[] s)
        {
            uint h = OffsetBasis;
            foreach (byte c in s)
                h = (h ^ c) * Prime;
            h *= 0x2001;
            h = (h ^ (h >> 0x7)) * 0x9;
            h = (h ^ (h >> 0x11)) * 0x21;
            return h;
        }
    }

    static class Utilities
    {
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
