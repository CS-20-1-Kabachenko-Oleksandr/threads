using threads;
public class EmulatorThreads
{
    public string inputPath;
    public string outputPath;
    public const string resultFileName = "/summary.json";
    public FileManager FileManager = new FileManager();
    public CreditCard CreditCard = new CreditCard();
    public WriteTransactions WriteTransactions = new WriteTransactions();

    public EmulatorThreads(string inputPath, string outputPath)
    {
        this.inputPath = inputPath;
        this.outputPath = outputPath;
    }

    public void Run()
    {
        FileManager.DeleteAllFiles(outputPath);
        string[] files = FileManager.GetSortedFiles(inputPath);
        List<Thread> threads = new List<Thread>();

        foreach (string file in files)
        {
            Thread thread = new Thread(ProcessTransaction);
            threads.Add(thread);
            thread.Start(file);
        }
        
        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        var TranscationSummary = new TransactionSummary(CreditCard.balance, CreditCard.successfulTransactions,
            CreditCard.nonSuccessfulTransactions, FileManager._processedFileCount);
        WriteTransactions.SaveObjectToJsonFile(TranscationSummary, outputPath + resultFileName);
    }

    public void ProcessTransaction(object state)
    {
        if (state is string filePath)
        {
            int transactionAmount = FileManager.ReadLastLineValue(filePath);
            bool isDeposit = transactionAmount > 0;
            bool success;
            if (isDeposit)
            {
                success = CreditCard.Deposit(Math.Abs(transactionAmount));
            }
            else
            {
                success = CreditCard.Withdraw(Math.Abs(transactionAmount));
            }

            WriteTransactions.AppendTransactionLog(outputPath, filePath, transactionAmount, success);
            Console.WriteLine($"File {filePath} processed. Success: {success}");
        }
    }
}