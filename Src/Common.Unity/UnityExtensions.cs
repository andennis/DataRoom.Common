using System;
using System.Linq;
using Common.Configuration;
using Common.Configuration.Unity;
using Unity;

namespace Common.Unity
{
    public static class UnityExtensions
    {
        public static void LoadUnityConfiguration(this IUnityContainer container, IUnityConfiguration unityConfig, string containerName)
        {
            UnityConfiguration cfg = unityConfig.Unity;
            UnityContainerConfiguration cfgContainer = cfg.Containers.FirstOrDefault(x => x.Container == containerName);
            if (cfgContainer == null)
                throw new AppConfigurationException($"Unity container configuration '{containerName}' does not exist");

            foreach (UnityMappingConfiguration cfgMapping in cfgContainer.Mappings)
            {
                if (string.IsNullOrEmpty(cfgMapping.Type))
                    throw new AppConfigurationException($"Unity container configuration '{containerName}', 'type' parameter in mappinng is not specified");
                if (string.IsNullOrEmpty(cfgMapping.MapTo))
                    throw new AppConfigurationException($"Unity container configuration '{containerName}', 'MapTo' parameter in mappinng is not specified");

                Type type = Type.GetType(cfgMapping.Type);
                Type mapTo = Type.GetType(cfgMapping.MapTo);

                container.RegisterType(type, mapTo);
            }
        }
    }
}
