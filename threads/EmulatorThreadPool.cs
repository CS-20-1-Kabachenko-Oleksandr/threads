using threads;
public class EmulatorThreadPool
{
    public string InputPath;
    public string OutputPath;
    public const string ResultFileName = "/summary.json";
    public FileManager FileManager = new FileManager();
    public CreditCard CreditCard = new CreditCard();
    public WriteTransactions WriteTransactions = new WriteTransactions();

    public EmulatorThreadPool(string inputPath, string outputPath)
    {
        InputPath = inputPath;
        OutputPath = outputPath;
    }

    public void Run()
    {
        FileManager.DeleteAllFiles(OutputPath);
        string[] files = FileManager.GetSortedFiles(InputPath);

        using (ManualResetEvent resetEvent = new ManualResetEvent(false))
        {
            foreach (string file in files)
            {
                ThreadPool.QueueUserWorkItem(ProcessTransaction, new Tuple<string, ManualResetEvent>(file, resetEvent));
            }

            resetEvent.WaitOne();
        }

        var transactionSummary = new TransactionSummary(CreditCard.balance, CreditCard.SuccessfulTransactions,
            CreditCard.nonSuccessfulTransactions, FileManager.ProcessedFileCount);
        WriteTransactions.SaveObjectToJsonFile(transactionSummary, OutputPath + ResultFileName);
    }

    public void ProcessTransaction(object state)
    {
        if (state is Tuple<string, ManualResetEvent> tuple)
        {
            try
            {
                int transactionAmount = FileManager.ReadLastLineValue(tuple.Item1);
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

                WriteTransactions.AppendTransactionLog(OutputPath, tuple.Item1, transactionAmount, success);
            }
            finally
            {
                if (Interlocked.Decrement(ref FileManager._processedFileCount) == 0)
                {
                    tuple.Item2.Set();
                }
            }
        }
    }
}
