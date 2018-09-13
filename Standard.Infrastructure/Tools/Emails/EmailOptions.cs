using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.Tools.Emails
{
    public class EmailOptions
    {
        public string Account { get; set; }

        public string Pass { get; set; }

        public string SmtpServer { get; set; }

        public string Name { get; set; }
    }
}
