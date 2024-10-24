public class Customer
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public string Address { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string BVN { get; set; }
    public string NIN { get; set; }
    public string AccountType { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public Customer()
    {

    }
    public Customer(string firstName, string lastName, string address, string bvn, string nin, string accountType, string email, string password , DateTime dateOfBirth, string gender)
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        Address = address;
        DateOfBirth = dateOfBirth;
        BVN = bvn;
        NIN = nin;
        AccountType = accountType;
        Email = email;
        Password = password;
    }
}
