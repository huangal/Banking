using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Banking.Enterprise.Configuration
{
    public class ConfigOptions<T> : IConfigOptions<T> where T : class
    {

        private IConfiguration Configuration;

        public ConfigOptions(IConfiguration configuration)
        {
            Configuration = configuration;


        }

        public T Value
        {
            get {

                var name = typeof(T).Name;
                return Configuration.GetSection(name).Get<T>();
            }
        }
    }

}
