using threads;

public class EmulatorTasks
{
    public string inputPath;
    public string outputPath;
    public const string resultFileName = "/summary.json";
    List<Task> tasks = new List<Task>();
    public FileManager FileManager = new FileManager();
    public CreditCard CreditCard = new CreditCard();
    public WriteTransactions WriteTransactions = new WriteTransactions();

    public EmulatorTasks(string inputPath, string outputPath)
    {
        this.inputPath = inputPath;
        this.outputPath = outputPath;
    }

    public async Task Run()
    {
        FileManager.DeleteAllFiles(outputPath);
        string[] files = FileManager.GetSortedFiles(inputPath);
        foreach (string file in files)
        {
            tasks.Add(ProcessTransaction(file));
        }
        await Task.WhenAll(tasks);

        var TranscationSummary = new TransactionSummary(CreditCard.balance, CreditCard.successfulTransactions, CreditCard.nonSuccessfulTransactions, FileManager._processedFileCount);
        WriteTransactions.SaveObjectToJsonFile(TranscationSummary, outputPath + resultFileName);
    }

    public async Task ProcessTransaction(string filePath)
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
    }
}