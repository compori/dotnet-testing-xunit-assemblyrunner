using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Compori.Testing.Xunit.AssemblyRunner
{
    /// <summary>
    /// Class FilterFile.
    /// </summary>
    public class FilterFile
    {
        /// <summary>
        /// Parses the specified line.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns>Filter.</returns>
        public Filter Parse(string line)
        {
            if(line == null)
            {
                return null;
            }

            line = line.Trim();

            //
            // Comment oder nothing.
            //
            if (line.StartsWith("#") || string.IsNullOrEmpty(line))
            {
                return null;
            }

            // Class.
            var classKey = "class:";
            if (line.StartsWith(classKey))
            {
                var testClass = line.Substring(classKey.Length).Trim();
                return new Filter
                {
                    ClassName = testClass
                };
            }

            // Case
            var caseKey = "case:";
            if (line.StartsWith(caseKey))
            {
                var testCase = line.Substring(caseKey.Length).Trim();
                return new Filter
                {
                    Case = testCase
                };
            }

            // Filter Traits
            var traitKey = "trait:";
            if (line.StartsWith(traitKey))
            {
                var testTrait = line.Substring(traitKey.Length).Trim();
                var split = testTrait.Split(new char[] { '=' }, 2);
                string splitKey = null;
                string splitValue = null;
                if (split.Length >= 1)
                {
                    splitKey = split[0].Trim();
                }
                if (split.Length == 2)
                {
                    splitValue = split[1].Trim();
                }
                var name = string.IsNullOrEmpty(splitKey) ? null : splitKey;
                var value = string.IsNullOrEmpty(splitValue) ? null : splitValue;

                if( name == null && value == null)
                {
                    return null;
                }
                return new Filter
                {
                    TraitName = name,
                    TraitValue = value
                };
            }

            return null;
        }

        /// <summary>
        /// Loads the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>List&lt;Filter&gt;.</returns>
        /// <exception cref="System.InvalidOperationException">File does not exists</exception>
        public List<Filter> Load(string file)
        {
            if (!File.Exists(file))
            {
                throw new InvalidOperationException("File does not exists");
            }
            var lines = File.ReadAllLines(file, Encoding.UTF8);
            var result = new List<Filter>();

            foreach (var line in lines)
            {
                var filter = this.Parse(line);
                if(filter != null)
                {
                    result.Add(filter);
                }                
            }
            return result;
        }
    }
}
