﻿namespace LoanManagement.Persistence.Ef.UnitOfWorks;

public class EfUnitOfWork(EfDataContext dbContext) : UnitOfWork
{
    public async Task SaveAsync()
    {
        await dbContext.SaveChangesAsync();
    }

    public void Save()
    {
        dbContext.SaveChanges();
    }

    public async Task Begin()
    {
        await dbContext.Database.BeginTransactionAsync();
    }

    public async Task Rollback()
    {
        await dbContext.Database.RollbackTransactionAsync();
    }

    public async Task Commit()
    {
        await dbContext.SaveChangesAsync();
        await dbContext.Database.CommitTransactionAsync();
    }
}