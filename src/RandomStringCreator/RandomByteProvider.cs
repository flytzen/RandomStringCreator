using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Security.Cryptography;


namespace RandomStringCreator 
{
    internal class RandomByteProvider 
    {
        private IEnumerator<byte> enumerator;
        object locker = new Object();
        private readonly int bufferLength;

        RandomNumberGenerator cryptoGen = RandomNumberGenerator.Create();

        public RandomByteProvider(int bufferLength) 
        {
            this.bufferLength = bufferLength;
            this.enumerator = this.GetBytes().GetEnumerator();
        }

        public IEnumerable<int> Bytes()
        {
            while(true)
            {
                // This locking doesn't actually make it 100% threadsafe - at least not in theory
                // But it does somehow seem to work...
                lock(locker) 
                {
                    if (this.enumerator.MoveNext())
                    {
                        yield return this.enumerator.Current;
                    } 
                    else 
                    {
                        break;
                    }
                }
            }
        }

        private IEnumerable<byte> GetBytes()
        {
            // http://stackoverflow.com/questions/38632735/rngcryptoserviceprovider-in-net-core
            // TODO utilise IDisposable
            // Gives BCryptGenRandom on Windows and 
            var bytes = new byte[this.bufferLength];

            while (true)
            {
                // Console.WriteLine("** Asking cryptogen for more bytes **");
                cryptoGen.GetBytes(bytes);
                for (int i = 0; i < this.bufferLength; i++)
                {
                    //Debug.WriteLine($"Returning byte {i}");
                    yield return bytes[i];
                }
            }
        }   
    }
}