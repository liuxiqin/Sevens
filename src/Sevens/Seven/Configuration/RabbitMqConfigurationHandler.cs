using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seven.Configuration
{
    public class RabbitMqConfigurationHandler : ConfigurationSection
    {
        [ConfigurationProperty("HostName", DefaultValue = "12.0.0.1", IsRequired = true, IsKey = true)]
        public string HostName
        {
            get { return this["HostName"].ToString(); }
            set { this["HostName"] = value; }
        }

        [ConfigurationProperty("VirtualName", DefaultValue = "/", IsRequired = true, IsKey = true)]
        public string VirtualName
        {
            get { return this["VirtualName"].ToString(); }
            set { this["VirtualName"] = value; }
        }

        [ConfigurationProperty("UserName", DefaultValue = "guest", IsRequired = true, IsKey = true)]
        public string UserName
        {
            get { return this["UserName"].ToString(); }
            set { this["UserName"] = value; }
        }

        [ConfigurationProperty("UserPaasword", DefaultValue = "guest", IsRequired = true, IsKey = true)]
        public string UserPaasword
        {
            get { return this["UserPaasword"].ToString(); }
            set { this["UserPaasword"] = value; }
        }

        [ConfigurationProperty("Port", DefaultValue = 5672, IsRequired = true, IsKey = true)]
        public int Port
        {
            get
            {
                if (string.IsNullOrEmpty(this["Port"].ToString())) return 5672;

                return int.Parse(this["Port"].ToString());
            }
            set { this["Port"] = value; }
        }
    }
}
