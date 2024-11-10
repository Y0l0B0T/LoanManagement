using LoanManagement.Persistence.Ef.UnitOfWorks;
using LoanManagement.Services.Admins;
using LoanManagement.Services.installments;
using LoanManagement.Services.Loans;
using LoanManagement.Services.LoansDefinition;
using LoanManagement.Services.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EfDataContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UnitOfWork, EfUnitOfWork>();
builder.Services.AddScoped<AdminService, AdminAppService>();
builder.Services.AddScoped<AdminQuery, EFAdminQuery>();
builder.Services.AddScoped<AdminRepository, EFAdminRepository>();
builder.Services.AddScoped<CustomerService, CustomerAppService>();
builder.Services.AddScoped<CustomerQuery, EFCustomerQuery>();
builder.Services.AddScoped<CustomerRepository, EFCustomerRepository>();
builder.Services.AddScoped<LoanDefinitionService, LoanDefinitionAppService>();
builder.Services.AddScoped<LoanDefinitionQuery, EFLoanDefinitionQuery>();
builder.Services.AddScoped<LoanDefinitionRepository, EFLoanDefinitionRepository>();
builder.Services.AddScoped<LoanService, LoanAppService>();
builder.Services.AddScoped<LoanQuery, EFLoanQuery>();
builder.Services.AddScoped<LoanRepository, EFLoanRepository>();
builder.Services.AddScoped<InstallmentService, InstallmentAppService>();
builder.Services.AddScoped<InstallmentQuery, EFInstallmentQuery>();
builder.Services.AddScoped<InstallmentRepository, EFInstallmentRepository>();
builder.Services.AddScoped<ReportQuery, EFReportQuery>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();