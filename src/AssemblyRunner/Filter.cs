﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Abstractions;

namespace Compori.Testing.Xunit.AssemblyRunner
{
    public class Filter
    {
        /// <summary>
        /// Gets the assembly location.
        /// </summary>
        /// <value>The assembly location.</value>
        public string AssemblyLocation { get; set; }

        /// <summary>
        /// Gets or sets the name of the case.
        /// </summary>
        /// <value>The name of the case.</value>
        public string Case { get; set; }

        /// <summary>
        /// Gets or sets the name of the class.
        /// </summary>
        /// <value>The name of the class.</value>
        public string ClassName { get; set; }

        /// <summary>
        /// Gets or sets the name of the trait.
        /// </summary>
        /// <value>The name of the trait.</value>
        public string TraitName { get; set; }

        /// <summary>
        /// Gets or sets the trait value.
        /// </summary>
        /// <value>The trait value.</value>
        public string TraitValue { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current <see cref="object" />.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Filter filter))
            {
                return false;
            }
            if(this.AssemblyLocation != null && !this.AssemblyLocation.Equals(filter.AssemblyLocation))
            {
                return false;
            }
            if (this.AssemblyLocation == null && filter.AssemblyLocation != null)
            {
                return false;
            }

            if (this.Case != null && !this.Case.Equals(filter.Case))
            {
                return false;
            }
            if (this.Case == null && filter.Case != null)
            {
                return false;
            }

            if (this.ClassName != null && !this.ClassName.Equals(filter.ClassName))
            {
                return false;
            }
            if (this.ClassName == null && filter.ClassName != null)
            {
                return false;
            }

            if (this.TraitName != null && !this.TraitName.Equals(filter.TraitName))
            {
                return false;
            }
            if (this.TraitName == null && filter.TraitName != null)
            {
                return false;
            }

            if (this.TraitValue != null && !this.TraitValue.Equals(filter.TraitValue))
            {
                return false;
            }

            if (this.TraitValue == null && filter.TraitValue != null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Matches the specified assembly location.
        /// </summary>
        /// <param name="assemblyLocation">The assembly location.</param>
        /// <param name="testCase">The test case.</param>
        /// <returns><c>true</c> if filter matches test case, <c>false</c> otherwise.</returns>
        public bool Match(string assemblyLocation, ITestCase testCase)
        {

            //
            // Test assembly location matching
            //
            if (!string.IsNullOrEmpty(this.AssemblyLocation) && !this.AssemblyLocation.Equals(assemblyLocation))
            {
                return false;
            }

            //
            // Test case matching
            //
            if (!string.IsNullOrEmpty(this.Case) && !this.Case.Equals(testCase.DisplayName))
            {
                return false;
            }

            //
            // Test class matching
            //
            if (!string.IsNullOrEmpty(this.ClassName) && !this.Case.Equals(testCase.TestMethod.TestClass.Class.Name))
            {
                return false;
            }

            //
            // Trait matching
            //
            if (!string.IsNullOrEmpty(this.TraitName))
            {
                if(!testCase.Traits.ContainsKey(this.TraitName))
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(this.TraitValue) && !testCase.Traits[this.TraitName].Contains(this.TraitValue))
                {
                    return false;
                }
            }

            // seems to fit.
            return true;
        }
    }
}
