using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Standard.Infrastructure.Tools.Emails
{
    public class Email : IEmail
    {
        readonly EmailOptions option;
        public Email(EmailOptions option)
        {
            this.option = option;
        }
        public async Task Send(string to, string subject, string content)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(option.Name,option.Account));
            message.To.Add(new MailboxAddress(to));
            message.Subject = subject;
            var plain = new TextPart(TextFormat.Plain)
            {
                Text = content
            };
            message.Body = plain;
            await SendAsyn(message);
        }
        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendAsyn(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                client.Connect(option.SmtpServer);
                await client.AuthenticateAsync(option.Account, option.Pass);
                await client.SendAsync(message);
            }
        }
    }
}

