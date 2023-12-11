using System;
using System.IO;
using Newtonsoft.Json;

namespace threads 
{
    public class WriteTransactions
    {

        public void AppendTransactionLog(string outputDirectory, string transactionFilePath, decimal transactionAmount, bool transactionSuccess)
        {
            var logFilePath = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(transactionFilePath));

            try
            {
                using (var logStreamWriter = new StreamWriter(logFilePath, true))
                {
                    logStreamWriter.WriteLine($"{transactionAmount},{transactionSuccess}");
                }
            }
            catch (Exception logException)
            {
                Console.WriteLine($"Error appending transaction log: {logException.Message}");
            }
        }

        public void SaveObjectToJsonFile(object dataObject, string targetFilePath)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(dataObject, Formatting.Indented);

                using (var fileWriter = new StreamWriter(targetFilePath))
                {
                    fileWriter.Write(jsonData);
                }
            }
            catch (Exception jsonException)
            {
                Console.WriteLine($"Error saving object to JSON file: {jsonException.Message}");
            }
        }
    }
}