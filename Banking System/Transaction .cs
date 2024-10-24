public class Transaction
{
    public DateTime TransactionDate { get; set; }
    public int SenderAccount { get; set; }
    public int RecieverAccount { get; set; }
    public string TransactionReference { get; set; }
    public string TransactionStatus { get; set; }
    public string Narration { get; set; }
    public decimal TrasactionAmount { get; set; }

    public Transaction()
    {
    }
    public Transaction(DateTime transactionDate, int senderAccount, int recieverAccount, string transactionReference, string transactionStatus, string narration, decimal trasactionAmount)
    {
        TransactionDate = transactionDate;
        SenderAccount = senderAccount;
        RecieverAccount = recieverAccount;
        TransactionReference = transactionReference;
        TransactionStatus = transactionStatus;
        Narration = narration;
        TrasactionAmount = trasactionAmount;
    }
}
