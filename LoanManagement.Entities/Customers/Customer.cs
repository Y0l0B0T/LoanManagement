namespace LoanManagement.Entities.Customers;

public class Customer
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string NationalCode { get; set; }
    public string? Email { get; set; }
    public string? Documents { get; set; }
    public bool IsVerified { get; set; } = false;
    public decimal? MonthlyIncome { get; set; }
    public JobType? JobType { get; set; }
    public decimal? Assets { get; set; }
    public HashSet<Loan> MyLoans { get; set; } = [];
}

public enum JobType
{
    Employee = 1,
    SelfEmployed = 2,
    UnEmployed = 3
}