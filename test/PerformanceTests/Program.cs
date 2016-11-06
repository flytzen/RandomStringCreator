namespace PerformanceTests
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Running;

    using RandomStringCreator;

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<GenerationTests>();
        }
    }

    public class GenerationTests
    {
        private Generator standardGenerator = new Generator();
        private Generator buffer1024Generator = new Generator(1024);
        private Generator buffer4096Generator = new Generator(4096);
        private Generator buffer100000Generator = new Generator(100000);

        public GenerationTests()
        {
            // Warm them up
            this.standardGenerator.Get(1);
            this.buffer1024Generator.Get(1);
            this.buffer4096Generator.Get(1);
            this.buffer100000Generator.Get(1);

        }

        [Benchmark]
        public string GenerateIndividually()
        {
           return new Generator().Get(6);
        }

        [Benchmark]
        public string GenerateWithSharedInstanceWithDefaultBuffer()
        {
           return this.standardGenerator.Get(6);
        }

        [Benchmark]
        public string GenerateWith1024Buffer()
        {
           return this.buffer1024Generator.Get(6);
        }

        [Benchmark]
        public string GenerateWith4096Buffer()
        {
           return this.buffer4096Generator.Get(6);
        }

        [Benchmark]
        public string GenerateWith100000Buffer()
        {
            return this.buffer100000Generator.Get(6);
        }
    }
}
