﻿using Shouldly;
using Stryker.Abstractions;
using Stryker.Core.MutantFilters;
using Stryker.Core.Mutants;
using Stryker.Core.Options;
using Xunit;

namespace Stryker.Core.UnitTest.MutantFilters
{
    public class ExcludeMutationMutantFilterTests
    {
        [Theory]
        [InlineData(Mutator.Arithmetic, true)]
        [InlineData(Mutator.Assignment, false)]
        public void MutantFilter_ShouldSkipMutationsForExcludedMutatorType(Mutator excludedMutator, bool skipped)
        {
            // Arrange
            var mutant = new Mutant
            {
                Mutation = new Mutation
                {
                    Type = Mutator.Arithmetic,
                }
            };

            var sut = new ExcludeMutationMutantFilter();

            // Act
            var filteredMutants = sut.FilterMutants(
                new[] { mutant },
                null,
                new StrykerOptions(excludedMutations: new[] { excludedMutator.ToString() }));

            // Assert
            if (skipped)
            {
                filteredMutants.ShouldNotContain(mutant);
            }
            else
            {
                filteredMutants.ShouldContain(mutant);
            }
        }
    }
}