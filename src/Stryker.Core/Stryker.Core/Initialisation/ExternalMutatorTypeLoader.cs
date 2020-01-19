using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using McMaster.NETCore.Plugins;
using Stryker.Abstractions;

namespace Stryker.Core.Initialisation
{
    public interface IExternalMutatorTypeLoader
    {
        IEnumerable<IMutator> LoadExternalMutators(string path);
    }

    public class ExternalMutatorTypeLoader : IExternalMutatorTypeLoader
    {
        private readonly IFileSystem _fileSystem;

        public ExternalMutatorTypeLoader(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public IEnumerable<IMutator> LoadExternalMutators(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(nameof(path));
            }

            var dlls = _fileSystem.Directory
                .EnumerateFiles(path, "*Stryker.Mutators.dll", SearchOption.TopDirectoryOnly);

            var loaders = dlls.Select(dll => PluginLoader
                .CreateFromAssemblyFile(
                    assemblyFile: dll,
                    config => config.PreferSharedTypes = true));

            var mutators = loaders.SelectMany(loader => loader
                .LoadDefaultAssembly()
                .GetTypes()
                .Where(t => typeof(IMutator).IsAssignableFrom(t))
                .Where(t => !t.IsAbstract))
                .Select(t => (IMutator)Activator.CreateInstance(t));

            return mutators;
        }
    }
}