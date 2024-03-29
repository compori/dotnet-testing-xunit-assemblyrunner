﻿using Compori.Testing.Xunit.AssemblyRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new RunnerFactory(new XunitAssemblyRunnerFactory());

            var location = typeof(SampleTest.MySampleTest).Assembly.Location;
            using (var sut = factory.Create(location))
            {
                sut.Execute();
                Console.WriteLine($"Total   : {sut.Summary.Total}");
                Console.WriteLine($"Failed  : {sut.Summary.Failed}");
                Console.WriteLine($"Skipped : {sut.Summary.Skipped}");
            }
            // SampleTest.MySampleTest.Context = "A sample value";
            // 
            // var location = new Uri(typeof(SampleTest.MySampleTest).Assembly.CodeBase).LocalPath;
        }
    }
}
