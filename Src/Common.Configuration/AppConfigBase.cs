using System;
using Common.Configuration.Unity;
using Common.Repository;
using Microsoft.Extensions.Configuration;

namespace Common.Configuration
{
    public abstract class AppConfigBase : IDbConfig, IUnityConfiguration
    {
        private readonly IConfiguration _rootConfig;
        private readonly IConfiguration _config;

        protected AppConfigBase(string sectionName = null, string appConfigFileName = "appsettings.json")
        {
            if (string.IsNullOrEmpty(appConfigFileName))
                throw new ArgumentNullException(nameof(appConfigFileName));

            var configBuilder = new ConfigurationBuilder();
            _rootConfig = configBuilder.AddJsonFile(appConfigFileName, optional: true).Build();
            _config = !string.IsNullOrEmpty(sectionName) ? _rootConfig.GetSection(sectionName) : _rootConfig;

            if (_config == null)
                throw new AppConfigurationException($"Configuration section '{sectionName}' does not exist");
        }

        public string GetConnectionString(string name)
        {
            return _rootConfig.GetConnectionString(name);
        }

        /// <summary>
        /// Configuration section should contain parameter "ConnectionStringName" which value specifies the name of connection string from the section "ConnectionStrings"
        /// </summary>
        public string ConnectionString
        {
            get
            {
                string name = GetValue<string>("ConnectionStringName");
                return GetConnectionString(name);
            }
        }

        public UnityConfiguration Unity
        {
            get
            {
                IConfigurationSection unity = _rootConfig.GetSection("Unity");
                if (unity == null)
                    throw new AppConfigurationException("Configuration section Unity does not exist");

                return unity.Get<UnityConfiguration>();
            }
        }

        protected TValue GetValue<TValue>(string key)
        {
            return _config.GetValue<TValue>(key);
        }

    }
}
