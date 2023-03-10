using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MG_Core.Services;

namespace MG_Core.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "确认您的邮箱",
                $"请点击此链接确认您的帐户： <a href='{HtmlEncoder.Default.Encode(link)}'>link</a><br/>如果无法点击,请复制以下链接到浏览器打开:{HtmlEncoder.Default.Encode(link)}<br/> 如果这不是您的行为请不要理会");
        }
    }
}
