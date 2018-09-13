using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Standard.Infrastructure.Tools.Emails
{
    public interface IEmail
    {
        Task Send(string to, string subject, string content);
    }
}
