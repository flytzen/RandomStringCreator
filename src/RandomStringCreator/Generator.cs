namespace RandomStringCreator
{
    using System;
    using System.Linq;

    public class Generator
    {
        private const string DefaultValid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private const int DefaultBufferLength = 128;

        private readonly string valid;

        private Lazy<RandomByteProvider> lazyByteProvider;

        private RandomByteProvider byteProvider => this.lazyByteProvider.Value;

        public Generator() : this(DefaultValid, DefaultBufferLength)
        {
        }

        public Generator(string valid) : this(valid, DefaultBufferLength)
        {}

        public Generator(int bufferLength) : this(DefaultValid, bufferLength)
        {}

        public Generator(string valid, int bufferLength)
        {
            // TODO: check that "valid" only contains ascii chars
            this.valid = valid;
            this.lazyByteProvider = new Lazy<RandomByteProvider>(() => new RandomByteProvider(bufferLength));
        }

        public string Get(int length)
        {
            var chars = this.GetChars(length);
            return new string(chars);
        }
        
        private char[] GetChars(int length)
        {
            // http://stackoverflow.com/questions/32932679/using-rngcryptoserviceprovider-to-generate-random-string
            return this.byteProvider.Bytes().Select(b => (char)b).Where(c => this.valid.Contains(c)).Take(length).ToArray();
        }
    }
}
