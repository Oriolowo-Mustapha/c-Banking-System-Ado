using MySql.Data.MySqlClient;
using System.Data;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Authentication;
using System.Xml.Linq;

public class AccountDB
{
    private static string ConnectionStringWithoutDB = "Server = localhost; User = root; password = password";
    private static string ConnectionString = "Server = localhost; User = root; database = Account; password = password";
    public static List<Account> accounts = new List<Account>();

    public static void CreateAccountDB()
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionStringWithoutDB))
        {
            connection.Open();
            string query = "Create Database if not exists Account";

            MySqlCommand command = new MySqlCommand(query, connection);
            var execute = command.ExecuteNonQuery();


        }
    }

    public static void CreateAccountTable()
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            connection.Open();
            string query = "CREATE TABLE IF NOT EXISTS Accounts(AccountNumber INT PRIMARY KEY NOT NULL, AccountHolder VARCHAR(255) NOT NULL, FOREIGN KEY (AccountHolder) REFERENCES Account.Customers(FirstName),Balance DECIMAL(12,4) NOT NULL,AccountType VARCHAR(255) NOT NULL,AccountStatus VARCHAR(255) NOT NULL, Pin int NOT NULL, DateOpened Date NOT NULL);";

            MySqlCommand command = new MySqlCommand(query, connection);
            var execute = command.ExecuteNonQuery();

        }
    }
    public static void RetrieveFromAccountDb()
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            connection.Open();
            string selectQuery = $"SELECT * From Accounts";
            using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Account account = new Account();
                        account.AccountNumber = reader.GetInt32(0);
                        account.AccountHolder = reader.GetString(1);
                        account.Balance = reader.GetDecimal(2);
                        account.AccountType = reader.GetString(3);
                        account.AccountStatus = reader.GetString(4);
                        account.Pin = reader.GetInt32(5);
                        account.DateOpened = reader.GetDateTime(6);
                        accounts.Add(account);
                    }
                }
            }
        }
    }


    public static void AddAccount()
    {
        Console.Write("Enter Account Holder => ");
        string name = Console.ReadLine();
        Random random = new Random();
        Account account = new Account();    
        foreach (var item in CustomerDB.customers)
        {
            if (name == item.FirstName)
            {
                int accountNumber = random.Next(11111, 99999);
                string accountStatus = "Active";
                decimal balance = 0.0M;
                int pin = 0;
                string accountType = item.AccountType;
                string accountHolder = item.FirstName;

                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    MySqlCommand insert = new MySqlCommand($"insert into Accounts (AccountNumber, AccountHolder, AccountType,AccountStatus,DateOpened,Balance,Pin) values('{account.AccountNumber = accountNumber}','{account.AccountHolder = accountHolder}','{account.AccountType = accountType}','{account.AccountStatus = accountStatus}',curdate(),'{account.Balance = balance}','{account.Pin = pin}');", connection);


                    var execute = insert.ExecuteNonQuery();

                    if (execute == 0)
                    {
                        Console.WriteLine("Account Couldnt Create.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Account Created Successfully\n");
                        CustomerDB.deletesubmitdetalis(name);
                        break;
                    }
                }
            }
        }
    }

    public static void DisableAccount()
    {
        Console.Write("Enter Account Holder => ");
        string name = Console.ReadLine();
        foreach (var item in CustomerDB.customers)
        {
            if (name == item.FirstName)
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = $"UPDATE Accounts SET AccountStatus = 'Blocked' where AccountHolder = '{name}'";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    var execute = command.ExecuteNonQuery();

                    if (execute > 0)
                    {
                        Console.WriteLine("Updated Successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Unable To Update.");
                    }
                }
            }
        }
    }

    public static void DeleteAccount()
    {
        Console.Write("Enter Account Holder => ");
        string name = Console.ReadLine();
        foreach (var item in CustomerDB.customers)
        {
            if (name == item.FirstName)
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = $"delete from Accounts where AccountHolder = '{name}';";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    var execute = command.ExecuteNonQuery();

                    if (execute > 0)
                    {
                        Console.WriteLine("Deleted Successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Unable To Delete.");
                    }
                }
            }
        }
        
    }

    public static void UpdatePin(string name)
    {
        Console.Write("Pin: ");
        int pin = int.Parse(Console.ReadLine());
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            connection.Open();
            string query = $"UPDATE Accounts SET Pin = '{pin}' where AccountHolder = '{name}'";

            MySqlCommand command = new MySqlCommand(query, connection);
            var execute = command.ExecuteNonQuery();

            if (execute > 0)
            {
                Console.WriteLine("Updated Successfully.");
            }
            else
            {
                Console.WriteLine("Unable To Update.");
            }
        }
    }

    public static void Deposit(string accountHolder)
    {
        foreach (var item in accounts)
        {
            if (item.AccountHolder == accountHolder)
            {
                if (item.AccountStatus != "Blocked")
                {
                    Console.WriteLine("How Much Do U Want To Deposit: ");
                    int amount = int.Parse(Console.ReadLine());

                    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                    {
                        connection.Open();
                        string query = $"UPDATE Accounts SET Balance = Balance + '{amount}' where AccountHolder = '{accountHolder}'";

                        MySqlCommand command = new MySqlCommand(query, connection);
                        var execute = command.ExecuteNonQuery();

                        if (execute > 0)
                        {
                            Console.WriteLine("Deposit Successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Unable To Deposit.");
                        }
                    }
                }
            }
        }
    }

    public static void Withdrawl(string accountHolder)
    {
        foreach (var item in accounts)
        {
            if (item.AccountHolder == accountHolder)
            {
                if (item.AccountStatus != "Blocked")
                {
                    Console.WriteLine("How Much Do U Want To Withdrawl: ");
                    int amount = int.Parse(Console.ReadLine());

                    if (item.Balance > amount || item.Balance < amount && item.AccountType == "Savings")
                    {
                        decimal overdraftlim = amount - item.Balance;
                        if (overdraftlim < 100000.00M)
                        {
                            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                            {
                                connection.Open();
                                string query = $"UPDATE Accounts SET Balance =  '{amount}' - Balance where AccountHolder = '{accountHolder}'";

                                MySqlCommand command = new MySqlCommand(query, connection);
                                var execute = command.ExecuteNonQuery();

                                if (execute > 0)
                                {
                                    Console.WriteLine("Withdrawl Successfully.");
                                }
                                else
                                {
                                    Console.WriteLine("Unable To Withdrawl.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Overdraft Limit Exceeded.");
                        }
                    }
                    else if(item.AccountType == "Checking")
                    {
                        if (amount > item.Balance)
                        {
                            Console.WriteLine("Insufficient Balance.\nCant Grant Overdraft For This Accout Type.");
                        }
                        else
                        {
                            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                            {
                                connection.Open();
                                string query = $"UPDATE Accounts SET Balance =  '{amount}' - Balance where AccountHolder = '{accountHolder}'";

                                MySqlCommand command = new MySqlCommand(query, connection);
                                var execute = command.ExecuteNonQuery();

                                if (execute > 0)
                                {
                                    Console.WriteLine("Withdrawl Successfully.");
                                }
                                else
                                {
                                    Console.WriteLine("Unable To Withdrawl.");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public static Account GetAccountFromAccountHolder(string accountHolder)
    {
        foreach (var item in accounts)
        {
            if (item.AccountHolder == accountHolder)
            {
                return item;
            }
        }
        return null;
    }

    public static void GetBalance(string accountHolder)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            connection.Open();

            string selectQuery = $"SELECT Balance From Accounts where AccountHolder = '{accountHolder}'";
            using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("Balance: ");
                    if (!reader.Read())
                    {
                        Console.WriteLine("Student Not Found");
                    }
                    else
                    {

                        Console.WriteLine($"Balance: {reader["Balance"]}");
                    }
                }
            }
        }
    }
    public static int GetAccNumByName(string accountHolder)
    {
        foreach (var item in accounts)
        {
            if (item.AccountHolder == accountHolder)
            {
                return item.AccountNumber;
            }
        }
        return 0;
    }

    public static void GetAccountInfo(string accountHolder)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            connection.Open();

            string selectQuery = $"SELECT * From Accounts where AccountHolder = '{accountHolder}'";
            using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        Console.WriteLine("Student Not Found");
                    }
                    else
                    {

                        Console.WriteLine($"Account Number: {reader["AccountNumber"]}, AccountHolder: {reader["AccountHolder"]}, Balance: {reader["Balance"]},AccountType: {reader["AccountType"]},AccountStatus: {reader["AccountStatus"]},Pin: {reader["Pin"]},DateOpened: {reader["DateOpened"]}");
                    }
                    
                }
            }
        }
    }

    public static void CheckBalance(string accountHolder)
    {
        foreach (var item in accounts)
        {
            if (item.AccountHolder == accountHolder)
            {
                if (item.Balance < 1000.00M)
                {
                    Console.WriteLine("YELLO U HAVE LOW BALANCE IN UR BANK PLS ENSURE U DEPOSIT.");
                }
            }
        }
    }
}
