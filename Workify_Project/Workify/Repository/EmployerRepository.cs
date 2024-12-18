using System;
using Microsoft.EntityFrameworkCore;
using Workify.Data;
using Workify.Models;

namespace Workify.Repository;

public class EmployerRepository : IEmployerRepository
{
    private readonly CareerCrafterDbContext _dbContext;

    public EmployerRepository(CareerCrafterDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Employer>> GetAllAsync()
    {
        return await _dbContext.Employers.ToListAsync();
    }

    public async Task<Employer?> GetByIdAsync(int id)
    {
        return await _dbContext.Employers.FindAsync(id);
    }

    public async Task AddAsync(Employer employer)
    {
        await _dbContext.Employers.AddAsync(employer);
        await _dbContext.SaveChangesAsync();
    }
    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Employer employer)
    {
        _dbContext.Employers.Update(employer);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var employer = await GetByIdAsync(id);
        if (employer != null)
        {
            _dbContext.Employers.Remove(employer);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<Employer?> GetByUserIdAsync(int userId)
    {
        return await _dbContext.Employers.FirstOrDefaultAsync(e => e.UserId == userId);
    }
}
