using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Stryker.Abstractions
{
    public interface IMutator
    {
        IEnumerable<Mutation> Mutate(SyntaxNode node);
    }
}
