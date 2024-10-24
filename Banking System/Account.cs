public class Account
{
    public int AccountNumber { get; set; }
    public int BVN { get; set; }
    public int NIN { get; set; }
    public decimal Balance { get; set; }
    public DateTime DateOpened { get; set; }
    public string AccountType { get; set; }
    public string AccountStatus { get; set; }
    public string AccountHolder { get; set; }
    public int Pin { get; set; }

    public Account()
    {

    }
    public Account(int accountNumber, int bvn, int nin, decimal balance, DateTime dateOpened, string accountType, string accountStatus, string accountHolder)
    {
        AccountNumber = accountNumber;
        BVN = bvn;  
        NIN = nin;
        Balance = balance;
        DateOpened = dateOpened;
        AccountType = accountType;
        AccountStatus = accountStatus;
        AccountHolder = accountHolder;
    }
    public Account(int accountNumber, int bvn, int nin, decimal balance, DateTime dateOpened, string accountType, string accountStatus, string accountHolder, int pin)
    {
        AccountNumber = accountNumber;
        BVN = bvn;
        NIN = nin;
        Balance = balance;
        DateOpened = dateOpened;
        AccountType = accountType;
        AccountStatus = accountStatus;
        AccountHolder = accountHolder;
        Pin = pin;
    }
    public Account(int pin)
    {
        Pin = pin;
    }
}
