using System.Collections.Generic;

namespace Common.Configuration.Unity
{
    public class UnityContainerConfiguration
    {
        public string Container { get; set; }
        public IList<UnityMappingConfiguration> Mappings { get; set; }
    }
}
