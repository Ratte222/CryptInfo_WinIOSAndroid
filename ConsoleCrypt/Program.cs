using Microsoft.Extensions.DependencyInjection;

namespace ConsoleCrypt
{
    class Program
    {
        public static ServiceProvider services;
        static void Main(string[] args)
        {
            Startup startup = new Startup();
            startup.ConfigureService(ref services);
            //IServiceScope scope = services.CreateScope();
            //scope.ServiceProvider.GetRequiredService<CommandInterpreter>();


            CommandInterpreter consoleInterpreter = services.GetService<CommandInterpreter>();
            


            if(args.Length > 0)
            {
                if (args[0].ToLower() == "loop")
                {
                    consoleInterpreter.Start();
                }
                else
                {
                    consoleInterpreter.InterpretCommand(args);
                }
            }
            else
            {
                consoleInterpreter.InterpretCommand(args);
            }
        }       
    }
}
