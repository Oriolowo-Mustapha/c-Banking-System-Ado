using MySql.Data.MySqlClient;
using System.Numerics;
using System.Security.Principal;
using System.Xml.Linq;

public class CustomerDB
{
    private static string ConnectionStringWithoutDB = "Server = localhost; User = root; password = password";
    private static string ConnectionString = "Server = localhost; User = root; database = Account; password = password";
    public static List<Customer> customers = new List<Customer>();

    public static void CreateCustomerTable()
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            connection.Open();
            string query = "CREATE TABLE IF NOT EXISTS Customers(FirstName VARCHAR(255) PRIMARY KEY NOT NULL, LastName VARCHAR(255) NOT NULL,Gender VARCHAR(255) NOT NULL , Address VARCHAR(255) NOT NULL,DateOfBirth Date  NOT NULL,BVN VARCHAR(255) NOT NULL, NIN VARCHAR(255) NOT NULL, AccountType VARCHAR(255) NOT NULL, Email VARCHAR(255) NOT NULL UNIQUE, Passwords VARCHAR(255) NOT NULL);";

            MySqlCommand command = new MySqlCommand(query, connection);
            var execute = command.ExecuteNonQuery();

        }
    }
    public static void CreateSubmitDetatilsTable()
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            connection.Open();
            string query = "CREATE TABLE IF NOT EXISTS SubmitDetails(FirstName VARCHAR(255) PRIMARY KEY NOT NULL, LastName VARCHAR(255) NOT NULL,Gender VARCHAR(255) NOT NULL , Address VARCHAR(255) NOT NULL,DateOfBirth Date NOT NULL,BVN VARCHAR(255) NOT NULL, NIN VARCHAR(255) NOT NULL, AccountType VARCHAR(255) NOT NULL, Email VARCHAR(255) NOT NULL UNIQUE, Passwords VARCHAR(255) NOT NULL);";

            MySqlCommand command = new MySqlCommand(query, connection);
            var execute = command.ExecuteNonQuery();

        }
    }
    public static Customer Login(string email, string passwords)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            connection.Open();
            string selectQuery = $"SELECT * From Customers where Email = '{email}' AND Passwords = '{passwords}'";
            using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }
                    else
                    {
                        Customer customer = new Customer();
                        customer.FirstName = reader.GetString(0);
                        customer.LastName = reader.GetString(1);
                        customer.Gender = reader.GetString(2);
                        customer.Address = reader.GetString(3);
                        customer.DateOfBirth = reader.GetDateTime(4);
                        customer.BVN = reader.GetString(5);
                        customer.NIN = reader.GetString(6);
                        customer.AccountType = reader.GetString(7);
                        customer.Email = reader.GetString(8);
                        customer.Password = reader.GetString(9);
                        return customer;
                    }
                }
            }
        }

 
        return null;
    }

    public static void SubmitDetails()
    {
        bool run = true;
        Customer customer = new Customer();
        while (run)
        {
            Console.WriteLine("Firstname => ");
            string firstName = Console.ReadLine();

            Console.WriteLine("Lastname => ");
            string lastName = Console.ReadLine();

            Console.WriteLine("Gender => ");
            string gender = Console.ReadLine();

            Console.WriteLine("Address => ");
            string address = Console.ReadLine();

            Console.Write("Date Of Birth (YYYY-MM-DD): ");
            string dateInput = Console.ReadLine();

            DateTime DOB;
            DateTime.TryParseExact(dateInput, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DOB);

            Console.Write("BVN => ");
            string bvn = Console.ReadLine();
            if (bvn.Length == 11)
            {
                Console.Write("NIN => ");
                string nin = Console.ReadLine();
                if (nin.Length == 11)
                {
                    Console.Write("Account Type\n1.Savings\n2.Checking\n=> ");
                    int num = int.Parse(Console.ReadLine());
                    if (num == 1)
                    {
                        string accountType = "Savings";
                        Console.Write("Email => ");
                        string email = Console.ReadLine();
                        if (email.Contains("@gmail.com"))
                        {
                            run = false;
                            Console.Write("Password => ");
                            string password = Console.ReadLine();
                            
                            
                            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                            {
                                connection.Open();
                               
                                MySqlCommand insert = new MySqlCommand($"insert into SubmitDetails (FirstName, LastName, Gender, Address, DateOfBirth, BVN, NIN, AccountType, Email, Passwords) values('{customer.FirstName = firstName}','{customer.LastName = lastName}','{customer.Gender = gender}','{customer.Address = address}', '{customer.DateOfBirth = DOB}','{customer.BVN = bvn}','{customer.NIN = nin}', '{customer.AccountType = accountType}','{customer.Email = email}','{customer.Password = password}');", connection);

                                MySqlCommand insert2 = new MySqlCommand($"insert into Customers (FirstName, LastName, Gender, Address, DateOfBirth, BVN, NIN, AccountType, Email, Passwords) values('{customer.FirstName = firstName}','{customer.LastName = lastName}','{customer.Gender = gender}','{customer.Address = address}', '{customer.DateOfBirth = DOB}','{customer.BVN = bvn}','{customer.NIN = nin}', '{customer.AccountType = accountType}','{customer.Email = email}','{customer.Password = password}');", connection);

                                var execute = insert.ExecuteNonQuery();
                                var execute2 = insert2.ExecuteNonQuery();

                                if (execute2 == 0)
                                {
                                    Console.WriteLine("REGISTERING FAILED");
                                }
                                else
                                {
                                    run = false;
                                    Console.WriteLine("REGISTERED SUCCESSFULY\nPLS AWAIT THE ADMIN FOR CREATION OF ACCOUNT\n");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("INVALID EMAIL FORMAT");
                            email = Console.ReadLine();
                        }
                    }
                    else if (num == 2)
                    {
                        string accountType = "Checking";
                        Console.Write("Email => ");
                        string email = Console.ReadLine();
                        if (email.Contains("@gmail.com"))
                        {
                            Console.Write("Password => ");
                            string password = Console.ReadLine();

                            run = false;
                            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                            {
                                connection.Open();
                                
                                MySqlCommand insert = new MySqlCommand($"insert into SubmitDetails (FirstName, LastName, Gender, Address, DateOfBirth, BVN, NIN, AccountType, Email, Passwords) values('{customer.FirstName = firstName}','{customer.LastName = lastName}','{customer.Gender = gender}','{customer.Address = address}', '{customer.DateOfBirth = DOB}','{customer.BVN = bvn}','{customer.NIN = nin}', '{customer.AccountType = accountType}','{customer.Email = email}','{customer.Password = password}');", connection);

                                MySqlCommand insert2 = new MySqlCommand($"insert into Customers (FirstName, LastName, Gender, Address, DateOfBirth, BVN, NIN, AccountType, Email, Passwords) values('{customer.FirstName = firstName}','{customer.LastName = lastName}','{customer.Gender = gender}','{customer.Address = address}', '{customer.DateOfBirth = DOB}','{customer.BVN = bvn}','{customer.NIN = nin}', '{customer.AccountType = accountType}','{customer.Email = email}','{customer.Password = password}');", connection);

                                var execute = insert.ExecuteNonQuery();
                                var execute2 = insert2.ExecuteNonQuery();

                                if (execute2 == 0)
                                {
                                    Console.WriteLine("REGISTERING FAILED");
                                }
                                else
                                {
                                    run = false;
                                    Console.WriteLine("REGISTERED SUCCESSFULY\nPLS AWAIT THE ADMIN FOR CREATION OF ACCOUNT\n");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("INVALID EMAIL FORMAT");
                            email = Console.ReadLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("INVALID INPUT.");
                        num = int.Parse(Console.ReadLine());
                    }
                }
                else
                {
                    Console.WriteLine("INCORRECT NIN LENGHT.\nMAKE SURE ITS 11");
                    Console.Write("NIN => ");
                    nin = Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("INCORRECT BVN LENGHT.\nMAKE SURE ITS 11");
                Console.Write("BVN => ");
                bvn = Console.ReadLine();
            }
        }
    }
    public static void deletesubmitdetalis(string name)
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            connection.Open();
            string query = $"delete from SubmitDetails where FirstName = '{name}';";

            MySqlCommand command = new MySqlCommand(query, connection);
            var execute = command.ExecuteNonQuery();
        }
    }
    public static void RetrieveFromSubmitedDb()
    {
        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
        {
            connection.Open();
            string selectQuery = $"SELECT * From SubmitDetails";
            using (MySqlCommand command = new MySqlCommand(selectQuery, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer customer = new Customer();
                        customer.FirstName = reader.GetString(0);
                        customer.LastName = reader.GetString(1);
                        customer.Gender = reader.GetString(2);
                        customer.Address = reader.GetString(3);
                        customer.DateOfBirth = reader.GetDateTime(4);
                        customer.BVN = reader.GetString(5);
                        customer.NIN = reader.GetString(6);
                        customer.AccountType = reader.GetString(7);
                        customer.Email = reader.GetString(8);
                        customer.Password = reader.GetString(9);
                        customers.Add(customer);
                    }
                }
            }
        }
    }

    public static void UpdateAccount(string bvn)
    {
        bool run = true;
        while (run)
        {
            Console.WriteLine("Firstname => ");
            string firstName = Console.ReadLine();

            Console.WriteLine("Lastname => ");
            string lastName = Console.ReadLine();

            Console.WriteLine("Address => ");
            string address = Console.ReadLine();

            Console.Write("Email => ");
            string email = Console.ReadLine();
            if (email.Contains("@gmail.com"))
            {
                run = false;
                Console.Write("Password => ");
                string password = Console.ReadLine();

                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = $"UPDATE Customers SET FirstName = '{firstName}',LastName = '{lastName}', Email = '{email}',Passwords = '{password}', Address = '{address}' where BVN = '{bvn}'";

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
            else
            {
                Console.WriteLine("Invalid E-mail Format.");
                Console.Write("Email => ");
                email = Console.ReadLine();
            }
        }
    }



}
