﻿using CommonForCryptPasswordLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;


namespace CommonForCryptPasswordLibrary.Services
{
    public class MyIO:IMyIO
    {
        public virtual void WriteLine(string content)
        {
            Console.WriteLine(content);
        }
        
        public virtual string ReadLine()
        {
            return Console.ReadLine();
        }
        public virtual void HandleMessage(string _msg, Exception ex)
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
#if DEBUG
            WriteLine(message);
#endif
        }
    }
}
