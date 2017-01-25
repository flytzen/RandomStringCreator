namespace RandomStringCreator
{
    using System;
    using System.Linq;

    public class StringCreator : IDisposable
    {
        private const string DefaultValid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private const int DefaultBufferLength = 128;

        private bool disposed = false;

        private readonly string valid;

        private Lazy<RandomByteProvider> lazyByteProvider;

        private RandomByteProvider byteProvider => this.lazyByteProvider.Value;

        public StringCreator() : this(DefaultValid, DefaultBufferLength)
        {
        }

        public StringCreator(string valid) : this(valid, DefaultBufferLength)
        {}

        public StringCreator(int bufferLength) : this(DefaultValid, bufferLength)
        {}

        public StringCreator(string valid, int bufferLength)
        {
            // TODO: check that "valid" only contains ascii chars
            this.valid = valid;
            this.lazyByteProvider = new Lazy<RandomByteProvider>(() => new RandomByteProvider(bufferLength));
        }

        public virtual string Get(int length)
        {
            var chars = this.GetChars(length);
            return new string(chars);
        }
        
        private char[] GetChars(int length)
        {
            // 
            return this.byteProvider.Bytes().Select(b => (char)b).Where(c => this.valid.Contains(c)).Take(length).ToArray();
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

            if (this.lazyByteProvider.IsValueCreated && this.lazyByteProvider.Value != null) 
            {
                this.lazyByteProvider.Value.Dispose();
            }

            disposed = true;
        }
    }
}
