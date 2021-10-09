using System;
using System.Collections.Generic;
using System.Text;
using CommonForCryptPasswordLibrary;
using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Services;
using ConsoleCrypt.DTO;

namespace ConsoleCrypt
{
    public interface ImyIO_Console: IMyIO
    {
        string ConsoleReadMultiline();
        void WriteLineUnknownCommand(string command);
        void WriteLineTooFewParameters();
        string GetHiddenInput();
        void Show(List<GroupDataDTO> groupDataDTOs);
        void Show(GroupDataDTO groupDataDTO, bool showGroupBlocks = true);
        void Show(List<BlockDataDTO> blockDataDTOs);
        void Show(BlockDataDTO blockDataDTO);
    }
    public class MyIO_Console:MyIO, ImyIO_Console
    {
        private const string startGroup = "------start group------";
        private const string endGroup = "------end group------";
        private const string startBlock = "------start block------";
        private const string endBlock = "------end block------";


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
#if DEBUG
            WriteLine(message);
#else
            WriteLine($"{_msg} {ex?.Message}");
#endif
        }

        public virtual void WriteLineUnknownCommand(string command)
        {
            Console.WriteLine($"->Unknown command. Use \"help {command}\" for give additional info");
        }
        public virtual void WriteLineTooFewParameters()
        {
            Console.WriteLine("->Too few parameters");
        }
        public virtual string GetHiddenInput()
        {
            StringBuilder input = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) break;
                if (key.Key == ConsoleKey.Backspace && input.Length > 0) input.Remove(input.Length - 1, 1);
                else if (key.Key == ConsoleKey.Backspace && input.Length == 0) /*Console.Write("\a");*/Console.Beep();
                else if (key.Key != ConsoleKey.Backspace) input.Append(key.KeyChar);
            }
            return input.ToString();
        }
        public string ConsoleReadMultiline()
        {
            Console.WriteLine("When you end - write \"end\". If you want to undo the input - write \"cancel\"");
            string line = "", result = "";
            do
            {
                //input code
                result += $"{line}\r\n";
                //Check for exit conditions
                line = $"{Console.ReadLine()}";
            } while (/*!String.IsNullOrWhiteSpace(line) && */!String.Equals(line.ToLower(), "end")
            && !String.Equals(line.ToLower(), "cancel"));
            if (String.Equals(line.ToLower(), "cancel"))
                return "";
            return result.TrimStart(new char[] { '\r', '\n' });
        }

        public void Show(List<GroupDataDTO> groupDataDTOs)
        {
            foreach (var group in groupDataDTOs)
            {
                //WriteLine(startGroup);
                Show(group);
                //WriteLine(endGroup);
                WriteLine("");
            }
        }

        public void Show(GroupDataDTO groupDataDTO, bool showGroupBlocks = true)
        {
            //if (showGroupBlocks)
            //{
            //    WriteLine(groupDataDTO.ToString());
            //    Show(groupDataDTO.CryptBlockModels);
            //}
            //else
            //{
            //    WriteLine(startGroup);
            //    WriteLine(groupDataDTO.ToString());
            //    WriteLine(endGroup);
            //    WriteLine("");
            //}
            WriteLine(startGroup);
            WriteLine(groupDataDTO.ToString());
            WriteLine(endGroup);
            WriteLine("");
            if (showGroupBlocks)
                Show(groupDataDTO.CryptBlockModels);

        }

        public void Show(List<BlockDataDTO> blockDataDTOs)
        {
            foreach (var block in blockDataDTOs)
            {
                Show(block);
            }
        }
        public void Show(BlockDataDTO blockDataDTO)
        {
            WriteLine(startBlock);
            WriteLine(blockDataDTO.ToString());
            WriteLine(endBlock);
            WriteLine("");       
        }
        
    }
}
