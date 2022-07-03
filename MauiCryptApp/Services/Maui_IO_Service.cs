using CommonForCryptPasswordLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.Services
{
    internal class Maui_IO_Service: CommonForCryptPasswordLibrary.Services.MyIO, IMyIO
    {
        public StringBuilder _content;
        public override void WriteLine(string content)
        {
            _content.AppendLine(content);
        }
        public override void HandleMessage(string _msg, Exception ex)
        {
            string message = "";
            if (ex != null)
            {
                message = $"{_msg} \r\n" +
                    $"Message = {ex?.Message?.ToString()} \r\n" +
                    $"InnerException = {ex?.InnerException?.ToString()} \r\n" +
                    $"Source = {ex?.Source?.ToString()} \r\n" +
                    $"StackTrace = {ex?.StackTrace?.ToString()} \r\n" +
                    $"TargetSite = {ex?.TargetSite?.ToString()}";
            }
            else
            {
                message = $"{_msg}";
            }
            _content.AppendLine(message);
        }
    }
}
