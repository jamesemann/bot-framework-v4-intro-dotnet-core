using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace dispatch
{
    public static class Config
    {   
        public static DispatchConfiguration GetDispatchConfiguration(IConfiguration configuration)
        {
            var dispatchConfig = new DispatchConfiguration();
            configuration.GetSection("Dispatch").Bind(dispatchConfig);
            return dispatchConfig;
        }
    }

    public class DispatchConfiguration
    {
        public DispatcherConfiguration Dispatcher { get; set; }
        public ModelConfiguration[] Models { get; set; }

        public class DispatcherConfiguration
        {
            public string Id { get; set; }
            public string Key { get; set; }
            public string Url { get; set; }

        }

        public class ModelConfiguration
        {
            public ModelTypes ModelType { get; set; }
            public string Name{ get; set; }
            public string Id { get; set; }
            public string Key { get; set; }
            public string Url { get; set; }
            
            public enum ModelTypes
            {
               qna,luis
            }
        }

    }
}
