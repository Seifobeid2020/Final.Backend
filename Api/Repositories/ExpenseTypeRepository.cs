using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Repositories
{
    public class ExpenseTypeRepository : IExpenseTypeRepository
    {
        private readonly ExpenseDbContext _context;

        public ExpenseTypeRepository(ExpenseDbContext context)
        {
            _context = context;
        }
        public async Task<ExpenseType> Add(ExpenseType expenseType)
        {
            _context.ExpenseTypes.Add(expenseType);
            await _context.SaveChangesAsync();
            Console.WriteLine(expenseType);
            return expenseType;
        }

        public async Task<ExpenseType> Delete(string UID, int id)
        {
            Console.WriteLine(id);
            var expenseType = await _context.ExpenseTypes.FindAsync(id);
            if (expenseType == null)
            {
                return null;
            }

            _context.ExpenseTypes.Remove(expenseType);
            await _context.SaveChangesAsync();
            return expenseType;
        }

        public async Task<ExpenseType> Get(string UID, int id)
        {
            var expenseType = await _context.ExpenseTypes
                                .Where(et => et.UserId.Contains(UID) && et.ExpenseTypeId.Equals(id))
                                .FirstOrDefaultAsync();
            return expenseType;
        }

        public async Task<List<ExpenseType>> GetAll(string UID)
        {
            return await _context.ExpenseTypes.Where(et=>et.UserId.Contains(UID)).ToListAsync();
        }

        public async Task<ExpenseType> Update(int id, ExpenseType expenseType)
        {
          

            _context.Entry(expenseType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseTypeExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            //check the return value
            Console.WriteLine(expenseType);

            return expenseType;
        }
        private bool ExpenseTypeExists(int id)
        {
            return _context.ExpenseTypes.Any(e => e.ExpenseTypeId == id);
        }
    }
}
