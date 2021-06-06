using System;
using Xunit;
using CommonForCryptPasswordLibrary;
using ConsoleCrypt;
using Moq;

namespace TestConsoleCrypt
{
    public class TestCommandInterpretator//nothing has been done and is not working 
    {
        [Fact]
        
        public void TestCommandHelp()
        {
            //Arrange
            var mockConsole_IO = new Mock<ImyIO_Console>();
            mockConsole_IO.Setup(cons => cons.WriteLine("Ok"));
            mockConsole_IO.Setup(cons =>  cons.ReadLine()).Returns<string>(answer =>"Ok");
            //MyIO_Console console_IO = new MyIO_Console();
            //Settings settings = new Settings(console_IO);
            
            var mockSettings = new Mock<ISettings>();
            mockSettings.Setup(setDef => setDef.GetDirCryptFile() == "D:\\Temp" 
            /*&& setDef.Get_caseSensitive() == true 
            && setDef.Get_searchInTegs() == true
            && setDef.Get_searchInHeader() == false
            && setDef.Get_searchUntilFirstMatch() == true
            && setDef.Get_viewServiceInformation() == true
            && setDef.Get_charStartAttributes() == "#^"*/);
            //var mockInputOutputFile = new Mock<I_InputOutput>();

            C_InputOutputFile InputOutputFile = new C_InputOutputFile(mockConsole_IO.Object, mockSettings.Object);
            //CommandInterpreter consoleInterpreter = new CommandInterpreter(mockInputOutputFile.Object,
            //    mockConsole_IO.Object, mockSettings.Object);
            //string[] command = new string[] { "help" };

            ////Act
            //bool result = consoleInterpreter.InterpretCommand(command);

            ////Assert
            Assert.True(true);
        }

        void WriteLine(string content)
        {

        }
    }
}
