﻿using LoanManagement.Entities.Loans;
using LoanManagement.Services.Loans.Contracts.DTOs;

namespace LoanManagement.Services.Report.Contracts.DTOs;

public class ReportActiveLoanDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string? LoanType { get; set; }
    public LoanStatus Status { get; set; }
    public decimal PaymentAmount { get; set; }
    public HashSet<GetPendingInstallmentsByLoanId> PendingInstallments { get; set; } = [];
}