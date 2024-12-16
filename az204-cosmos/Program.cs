using dotenv.net;
using Microsoft.Azure.Cosmos;

namespace az204cosmos
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                WriteLine("Beginning operations...\n");
                DotEnv.Load();
                var t = new Cosmos();
                await t.Run();

            }
            catch (CosmosException de)
            {
                var baseException = de.GetBaseException();
                WriteLine("{0} error occurred: {1}", de.StatusCode, de);
            }
            catch (Exception e)
            {
                WriteLine("Error: {0}", e);
            }
            finally
            {
                WriteLine("End of program, press any key to exit.");
                _ = ReadKey();
            }
        }
    }
}