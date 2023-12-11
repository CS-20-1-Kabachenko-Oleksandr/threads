namespace threads;

public class TransactionSummary
{
        public int EndingBalance { get; set; }
        public int SuccessfulTransactionCount { get; set; }
        public int UnsuccessfulTransactionCount { get; set; }
        public int ProcessedFileCount { get; set; }

        public TransactionSummary(int endingBalance, int successfulTransactionCount, int unsuccessfulTransactionCount, int processedFileCount)
        {
            EndingBalance = endingBalance;
            SuccessfulTransactionCount = successfulTransactionCount;
            UnsuccessfulTransactionCount = unsuccessfulTransactionCount;
            ProcessedFileCount = processedFileCount;
        }
}