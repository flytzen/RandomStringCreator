using System;
using System.Collections.Generic;
using System.Security.Cryptography;


namespace RandomStringCreator 
{
    internal class RandomByteProvider : IDisposable
    {
        bool disposed = false;
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
            // 
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); 
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return; 

            if (this.cryptoGen != null) 
            {
                this.cryptoGen.Dispose();
            }

            disposed = true;
        }
    }
}