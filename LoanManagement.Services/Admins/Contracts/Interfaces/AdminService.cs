﻿using LoanManagement.Service.Unit.Tests.Admins;

namespace LoanManagement.Services.Admins.Contracts.Interfaces;

public interface AdminService
{
    void Add(AddAdminDto dto);
    void Delete(int id);
}