﻿using Microsoft.EntityFrameworkCore;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Common;
using Shift_System.Persistence.Contexts;

namespace Shift_System.Persistence.Repositories
{
   public class GenericRepository<T> : IGenericRepository<T> where T : BaseAuditableEntity
   {
      private readonly ApplicationDbContext _dbContext;

      public GenericRepository(ApplicationDbContext dbContext)
      {
         _dbContext = dbContext;
      }

      public IQueryable<T> Entities => _dbContext.Set<T>();

      public async Task<T> AddAsync(T entity)
      {
         await _dbContext.Set<T>().AddAsync(entity);
         return entity;
      }

      public Task UpdateAsync(T entity)
      {
         T exist = _dbContext.Set<T>().Find(entity.Id);
         _dbContext.Entry(exist).CurrentValues.SetValues(entity);
         return Task.CompletedTask;
      }

      public Task DeleteAsync(T entity)
      {
         _dbContext.Set<T>().Remove(entity);
         return Task.CompletedTask;
      }

      public async Task<List<T>> GetAllAsync()
      {
         return await _dbContext
             .Set<T>()
             .ToListAsync();
      }

      public async Task<T> GetByIdAsync(Guid id)
      {
         return await _dbContext.Set<T>().FindAsync(id);
      }
   }
}
