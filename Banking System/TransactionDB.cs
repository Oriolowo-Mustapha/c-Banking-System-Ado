using MySql.Data.MySqlClient;
using System.Net.NetworkInformation;
using System.Transactions;
using System.Xml.Linq;

public class TransactionDB
{
    private static string ConnectionStringWithoutDB = "Server = localhost; User = root; password = password";
    private static string ConnectionString = "Server = localhost; User = root; database = Account; password = password";
    public static List<Transaction> transactions = new List<Transaction>();


    public static void CreateTransactionTable()
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            connection.Open();
            string query = "CREATE TABLE IF NOT EXISTS Transactions(TransactionDate Date PRIMARY KEY NOT NULL, SenderAccount INT NOT NULL, FOREIGN KEY (SenderAccount) REFERENCES Account.Accounts(AccountNumber),RecieverAccount INT NOT NULL, FOREIGN KEY (RecieverAccount) REFERENCES Account.Accounts(AccountNumber),TransactionReference VARCHAR(255) NOT NULL,TransactionStatus VARCHAR(255) NOT NULL,Narration VARCHAR(255) NOT NULL, TrasactionAmount DECIMAL(12,4) NOT NULL);";

            MySqlCommand command = new MySqlCommand(query, connection);
            var execute = command.ExecuteNonQuery();

        }
    }
    public static void RetrieveFromTransactionDb()
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            connection.Open();
            string selectQuery = $"SELECT * From Transactions";
            using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Transaction transaction = new Transaction();
                        transaction.TransactionDate = reader.GetDateTime(0);
                        transaction.SenderAccount = reader.GetInt32(1);
                        transaction.RecieverAccount = reader.GetInt32(2);
                        transaction.TransactionReference = reader.GetString(3);
                        transaction.TransactionStatus = reader.GetString(4);
                        transaction.Narration = reader.GetString(5);
                        transaction.TrasactionAmount = reader.GetDecimal(6);
                        transactions.Add(transaction);
                    }
                }
            }
        }
    }
    public static void MakeTransactions(Account account)
    {
        Transaction transaction = new Transaction();
        int senderAccount = account.AccountNumber;
        Console.Write("Enter Recievers Account Number => ");
        int recieversAccount = int.Parse(Console.ReadLine());

        foreach (var item in AccountDB.accounts)
        {
            if (item.AccountNumber == senderAccount)
            {
                Console.Write("Amount: ");
                decimal amount = decimal.Parse(Console.ReadLine());
                if (account.AccountType == "Savings")
                {
                    foreach (var item1 in AccountDB.accounts)
                    {
                        if (item1.AccountNumber == recieversAccount)
                        {
                            decimal balance = item1.Balance + amount;
                            Guid guid = new Guid();
                            string transactionReference = guid.ToString();

                            string transactionStatus = "Successful";

                            Console.Write("Narration => ");
                            string narration = Console.ReadLine();

                            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                            {
                                connection.Open();
                                MySqlCommand insert = new MySqlCommand($"insert into Transactions(TransactionDate, SenderAccount, RecieverAccount,TransactionReference,TransactionStatus,Narration,TrasactionAmount) values(curdate(),'{transaction.SenderAccount = senderAccount}','{transaction.RecieverAccount = recieversAccount}','{transaction.TransactionReference = transactionReference}','{transaction.TransactionStatus = transactionStatus}','{transaction.Narration = narration}','{transaction.TrasactionAmount = amount}');", connection);


                                var execute = insert.ExecuteNonQuery();
                                string query = $"UPDATE Accounts SET Balance = '{balance}' where AccountNumber = '{recieversAccount}'";

                                MySqlCommand command = new MySqlCommand(query, connection);
                                var execute2 = command.ExecuteNonQuery();

                                string query2 = $"UPDATE Accounts SET Balance = Balance - '{amount}' where AccountNumber = '{senderAccount}'";

                                MySqlCommand comman2 = new MySqlCommand(query2, connection);
                                var execute3 = comman2.ExecuteNonQuery();
                                if (execute == 0)
                                {
                                    Console.WriteLine("Transfer Declined.");
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Transfer Successfully\n");
                                    break;
                                }
                            }
                        }
                    }
                }
                else if(amount <= account.Balance)
                {
                    foreach (var item1 in AccountDB.accounts)
                    {
                        if (item1.AccountNumber == recieversAccount)
                        {
                            decimal balance = item1.Balance + amount;
                            Guid guid = new Guid();
                            string transactionReference = guid.ToString();

                            string transactionStatus = "Successful";

                            Console.Write("Narration => ");
                            string narration = Console.ReadLine();

                            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                            {
                                connection.Open();
                                MySqlCommand insert = new MySqlCommand($"insert into Transactions(TransactionDate, SenderAccount, RecieverAccount,TransactionReference,TransactionStatus,Narration,TrasactionAmount) values(curdate(),'{transaction.SenderAccount = senderAccount}','{transaction.RecieverAccount = recieversAccount}','{transaction.TransactionReference = transactionReference}','{transaction.TransactionStatus = transactionStatus}','{transaction.Narration = narration}','{transaction.TrasactionAmount = amount}');", connection);


                                var execute = insert.ExecuteNonQuery();
                                string query = $"UPDATE Accounts SET Balance = '{balance}' where AccountNumber = '{recieversAccount}'";

                                MySqlCommand command = new MySqlCommand(query, connection);
                                var execute2 = command.ExecuteNonQuery();

                                string query2 = $"UPDATE Accounts SET Balance = Balance - '{amount}' where AccountNumber = '{senderAccount}'";

                                MySqlCommand comman2 = new MySqlCommand(query2, connection);
                                var execute3 = comman2.ExecuteNonQuery();
                                if (execute == 0)
                                {
                                    Console.WriteLine("Transfer Declined.");
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Transfer Successfully\n");
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Insufficient Funds.\nPls Try again.");
                }
            }
        }  
    }

    public static void GetAllTransactionsByUser(int accNum)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            connection.Open();

            string selectQuery = $"SELECT * From Transactions where SenderAccount = {accNum}";
            using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("Transactions: ");
                    while (reader.Read())
                    {
                        Console.WriteLine($"Date: {reader["TransactionDate"]}, Sender's Account: {reader["SenderAccount"]}, Reciever's Account: {reader["RecieverAccount"]},Reference Number: {reader["TransactionReference"]}, Status: {reader["TransactionStatus"]},Narration: {reader["Narration"]},Narration: {reader["Narration"]}, Amount: {reader["TrasactionAmount"]}");
                    }
                }
            }
        }
    }
}
