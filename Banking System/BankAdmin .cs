public class BankAdmin
{
    public string Email { get; set; }
    public string Password { get; set; }

    public BankAdmin()
    {
        Email = "admin@gmail.com";
        Password = "admin";
    }


    public static BankAdmin Login(string email, string password)
    {
        BankAdmin admin = new BankAdmin();
        if (admin.Email == email && admin.Password == password)
        {
            return admin;
        }
        return null;
    }
}

