using System.Security.Cryptography;

namespace threads
{
    public class Program
    {
        public static async Task Main()
        {
            var input = "../../../input/";
            var output = "../../../output/";
            GenerateTestData generateTestTransactions = new GenerateTestData(5, input);
            generateTestTransactions.GenerateTransactions();
            
            //EmulatorTasks emulatorTasks = new EmulatorTasks(input, output);
            //await emulatorTasks.Run();

            //EmulatorThreadPool emulatorThreadPool = new EmulatorThreadPool(input, output);
            //emulatorThreadPool.Run();

            EmulatorThreads emulatorThreads = new EmulatorThreads(input, output);
            emulatorThreads.Run();
        }
    }
}