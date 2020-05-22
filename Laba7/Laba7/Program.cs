using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Laba7
{
    class Program
    {
        static uint _nonce = 1535000000; //1500000000;1540236492;

        static SHA256Managed _hasher = new SHA256Managed();
        static long _nonceOffset;

        static byte[] Current;
        private static DateTime _lastPrint = DateTime.Now;
        static uint _batchSize = 100000;
        static int printcounter = 0;
        static byte[] doubleHash;
        static void Main(string[] args)
        {
            string data = "010000001e60224709df1feb2e2849b7b10570abf7d4355ba8e2f6df121100000000000028cc65b7be2f8a1edc2af86ef369472443a1b70479cee205e8db5440cfbe943f57fad74df2b9441acc24ce5b";
            int g= Mining(Utils.ToBytes(data));
            Console.WriteLine();
            Console.ReadLine();
        }

        static int Mining(byte[] Data)
        {
            Current = (byte[])Data.Clone();
            _nonceOffset = Data.Length - 4;
            Console.WriteLine("Data: " + Utils.ToString(Data));
            Console.WriteLine("Start Mining");
            while (true)
            {
                if (GetHash(_batchSize) == 1)
                {
                    printData();
                    break;
                }
                else if (GetHash(_batchSize) == 0)
                {
                    PrintCurrentState();
                }
                else
                {
                    break;
                }

            }
            return 0;
        }


        static int GetHash(uint  n)
        {
            for (int nn = 0; nn < n; nn++)
            {
                BitConverter.GetBytes(_nonce).CopyTo(Current, _nonceOffset);
                doubleHash = Sha256(Sha256(Current));
                int zeroBytes = 0;
                for (int i = 31; i >= 28; i--)
                    zeroBytes += doubleHash[i];
                //Console.WriteLine(_nonce.ToString() +  "\t\t" + Utils.ToString(doubleHash));
                if (zeroBytes == 0)
                    return 1;
                if (_nonce == int.MaxValue)
                    return -1;
                _nonce++;
            }
            return 0;
        }
        static byte[] Sha256(byte[] input)
        {
            byte[] crypto = _hasher.ComputeHash(input, 0, input.Length);
            return crypto;

        }
       
        private static void PrintCurrentState()
        {
            if (printcounter != 10)
            {
                printcounter += 1;
                return;
            }
            printcounter = 0;
            double progress = ((double)_nonce / uint.MaxValue) * 100;
            //
            TimeSpan span = DateTime.Now - _lastPrint;
            Console.WriteLine("Speed: " + (int)(((_batchSize) / 100) / span.TotalSeconds) + "Kh/sec " + "Share progress:" + progress.ToString("F2") + "%");
            _lastPrint = DateTime.Now;
        }

        private static void printData()
        {
            Console.WriteLine("[Succes]Share found!");
            Console.WriteLine("Share: " + Utils.ToString(Current));
            Console.WriteLine("Nonce: " + Utils.ToString(_nonce));
            Console.WriteLine("Hash: " + Utils.ToString(doubleHash.Reverse().ToArray()));
        }
    }

    class Utils
    {

        public static byte[] ToBytes(string input)
        {
            byte[] bytes = new byte[input.Length / 2];
            for (int i = 0, j = 0; i < input.Length; j++, i += 2)
                bytes[j] = byte.Parse(input.Substring(i, 2), System.Globalization.NumberStyles.HexNumber);

            return bytes;
        }

        public static string ToString(byte[] input)
        {
            string result = "";
            foreach (byte b in input)
                result += b.ToString("x2");

            return result;
        }

        public static string ToString(uint value)
        {
            string result = "";
            foreach (byte b in BitConverter.GetBytes(value))
                result += b.ToString("x2");

            return result;
        }

        public static string EndianFlip32BitChunks(string input)
        {
            //32 bits = 4*4 bytes = 4*4*2 chars
            string result = "";
            for (int i = 0; i < input.Length; i += 8)
                for (int j = 0; j < 8; j += 2)
                {
                    //append byte (2 chars)
                    result += input[i - j + 6];
                    result += input[i - j + 7];
                }
            return result;
        }

    }
}