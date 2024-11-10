namespace LoanManagement.Services.Admins.Contracts.DTOs;

public class AddAdminDto
{
    [MinLength(3)]
    public required string Name { get; set; }
}