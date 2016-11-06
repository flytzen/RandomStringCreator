using System;
using Xunit;
using System.Threading;
using System.Collections.Generic;
using RandomStringCreator;

namespace Tests
{
    using System.Diagnostics;

    public class Tests
    {
        [Fact]
        public void CanGenerateA25CharacterString() 
        {
            using(var gen = new RandomStringCreator.StringCreator()) 
            {
                var randomString = gen.Get(25);
                Debug.WriteLine(randomString);

                Assert.True(randomString.Length == 25);
            }
        }


        // [Fact]
        // public void Dummy1()
        // {
        //     var gen = new RandomStringCreator.Generator();
        //     for(int i = 0; i < 10; i++)
        //     {
        //         Console.WriteLine(gen.Get(25));
        //     }
        // }

        // [Fact]
        // public void Dummy2() 
        // {
        //     var threads = new List<Thread>();
        //     var gen = new RandomStringCreator.Generator();
        //     for(int i = 0; i < 10; i++)
        //     {
        //         threads.Add(new Thread(() => {
        //             Console.WriteLine(gen.Get(25));
        //         }));
        //     }
        //     foreach(var t in threads)
        //     {
        //         t.Start();
        //     }
        //     foreach(var t in threads)
        //     {
        //         t.Join();
        //     }
        // }
    }
}
