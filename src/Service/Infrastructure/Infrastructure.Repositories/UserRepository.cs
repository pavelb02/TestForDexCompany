using Application.Services.DTO;
using Application.Services.Enums;
using Application.Services.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Exceptions;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TestForDexCompanyDbContext _dbContext;

    public UserRepository(TestForDexCompanyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        return await Task.FromResult(user.Id);
    }

    public async Task<User> GetByIdAsync(Guid userId, bool trackChanges)
    {
        var query = _dbContext.Users.Include(p => p.Advertisements).AsQueryable();

        if (!trackChanges)
            query = query.AsNoTracking();

        var user = await query.FirstOrDefaultAsync(c => c.Id == userId);

        if (user == null)
        {
            throw new EntityNotFoundException($"Пользователь с Id {userId} не найден.");
        }

        return user;
    }

    public Task Delete(User user)
    {
        _dbContext.Users.Remove(user);
        return Task.CompletedTask;
    }
}