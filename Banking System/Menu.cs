public class  Menu
{
    public static void MainMenu()
    {
        Console.WriteLine("1. REGISTER AS CUSTOMER.");
        Console.WriteLine("2. LOGIN.");
        Console.WriteLine("3. EXIT.");
        Console.Write("CHOOSE ANY OF THE FOLLOWING OPTIONS TO CONTINUE=> ");
        int input = int.Parse(Console.ReadLine());
        bool running = true;
        while (running)
        {
            switch (input)
            {
                case 1:
                    CustomerDB.SubmitDetails();
                    break;
                case 2:
                    AllocateEmail();
                    break;
                case 3:
                    running = false;
                    break;
                default:
                    Console.WriteLine("INVALID INPUT.");
                    Console.Write("CHOOSE ANY OF THE FOLLOWING OPTIONS TO CONTINUE=> ");
                    input = int.Parse(Console.ReadLine());
                    break;
            }
        }
    }

    public static void AllocateEmail()
    {
        Console.Write("Email => ");
        string email = Console.ReadLine();

        Console.Write("Password => ");
        string password = Console.ReadLine();
        var loggedInCustomer = CustomerDB.Login(email, password);
        var loggedInAdmin = BankAdmin.Login(email, password);
        bool run = true;
        while (run)
        {
            if (loggedInAdmin != null)
            {
                AdminMenu(loggedInAdmin);
                break;
            }
            else if(loggedInCustomer != null)
            {
                CustomerMenu(loggedInCustomer);
                break;
            }
            else
            {
                Console.WriteLine("INVALID INPUT.");
                AllocateEmail();
            }
        }
        
    }
    public static void CustomerMenu(Customer customer)
    {
        bool run = true;
        while (run)
        {
            AccountDB.CheckBalance(customer.FirstName);
            Console.WriteLine("1. SET UP PIN.");
            Console.WriteLine("2. CHANGE PIN.");
            Console.WriteLine("3. DEPOSIT.");
            Console.WriteLine("4. WITHDRWAL.");
            Console.WriteLine("5. MAKE TRANSACTION.");
            Console.WriteLine("6. VIEW BALANCE.");
            Console.WriteLine("7. VIEW TRANSACTION HISTORY.");
            Console.WriteLine("8. VIEW ACCOUNT INFO.");
            Console.WriteLine("9. LOGOUT.");
            Console.Write("CHOOSE ONE OF THE FOLLOWING OPTIONS: ");
            int opt = int.Parse(Console.ReadLine());
            switch (opt)
            {
                case 1:
                    AccountDB.UpdatePin(customer.FirstName);
                    break;
                case 2:
                    AccountDB.UpdatePin(customer.FirstName);
                    break;
                case 3:
                    AccountDB.Deposit(customer.FirstName);
                    break;
                case 4:
                    AccountDB.Withdrawl(customer.FirstName);
                    break;
                case 5:
                    TransactionDB.MakeTransactions(AccountDB.GetAccountFromAccountHolder(customer.FirstName)); 
                    break;
                case 6:
                    AccountDB.GetBalance(customer.FirstName);
                    break;
                case 7:
                    TransactionDB.GetAllTransactionsByUser(AccountDB.GetAccNumByName(customer.FirstName));
                    break;
                case 8:
                    AccountDB.GetAccountInfo(customer.FirstName);
                    break;
                case 9:
                    run = false;
                    break;
                 default:
                    Console.WriteLine("INVALID INPUT.");
                    break;
            }
        }

    }

    public static void AdminMenu(BankAdmin admin)
    {
        bool run = true;
        while (run)
        {
            foreach (var item in CustomerDB.customers)
            {
                Console.WriteLine($"{item.FirstName} HAS SUBMITTED DETAILS PLS CREATE ACCOUNT FOR CUSTOMER.");
            }
            Console.WriteLine("1. ADD CUSTOMER ACCOUNT.");
            Console.WriteLine("2. DISABLE CUSTOMER ACCOUNT.");
            Console.WriteLine("3. DELETE CUSTOMER ACCOUNT.");
            Console.WriteLine("4. GENERATE TRANSACTION REPORTS.");
            Console.WriteLine("5. LOGOUT.");
            Console.Write("CHOOSE ONE OF THE FOLLOWING OPTIONS: ");
            int opt = int.Parse(Console.ReadLine());
            switch (opt)
            {
                case 1:
                    AccountDB.AddAccount();
                    run = false;
                    break;
                case 2:
                    AccountDB.DisableAccount();
                    run = false;
                    break;
                case 3:
                    AccountDB.DeleteAccount();
                    run = false;
                    break;
                case 4:
                    Console.Write("Enter User Account Number => ");
                    int accNum = int.Parse(Console.ReadLine());
                    TransactionDB.GetAllTransactionsByUser(accNum);
                    break;
                case 5:
                    run = false;
                    break;
                default:
                    Console.WriteLine("INVALID INPUT.");
                    break;
            }
        }

    }
   
}
