// Global using directives

global using System.Transactions;
global using LoanManagement.Entities.Admins;
global using LoanManagement.Entities.Customers;
global using LoanManagement.Entities.installments;
global using LoanManagement.Entities.Loans;
global using LoanManagement.Persistence.Ef;
global using LoanManagement.Persistence.Ef.Admins;
global using LoanManagement.Persistence.Ef.Customers;
global using LoanManagement.Persistence.Ef.installments;
global using LoanManagement.Persistence.Ef.Loans;
global using LoanManagement.Persistence.Ef.LoansDefinition;
global using LoanManagement.Persistence.Ef.UnitOfWorks;
global using LoanManagement.Services.Admins;
global using LoanManagement.Services.Customers;
global using LoanManagement.Services.LoansDefinition;
global using LoanManagement.Services.LoansDefinition.Contracts.Interfaces;
global using LoanManagement.TestTools.Infrastructure.DataBaseConfig.Integration.Fixtures;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Xunit;