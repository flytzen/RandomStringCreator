using System;
using RandomStringCreator;

namespace Perftest2
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Running;

    class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<GenerationTests>();
        }
    }

    public class GenerationTests
    {
        private StringCreator standardGenerator = new StringCreator();

        private StringCreator buffer1024Generator = new StringCreator(1024);

        private StringCreator buffer4096Generator = new StringCreator(4096);

        private StringCreator buffer100000Generator = new StringCreator(100000);

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
            return new StringCreator().Get(6);
        }

        [Benchmark]
        public string GenerateIndividuallyWithUsing()
        {
            using (var generator = new StringCreator())
            {
                return generator.Get(6);
            }
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
