using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.Caches
{
    /// <summary>
    /// redis����
    /// </summary>
    public class RedisOptions
    {
        public string Port { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

    }
}
