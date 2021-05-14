using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Repositories
{
    public interface IExpenseTypeRepository
    {
        Task<List<ExpenseType>> GetAll(string UID);
        Task<ExpenseType> Get(string UID, int id);
        Task<ExpenseType> Add(ExpenseType expenseType);
        Task<ExpenseType> Update(int id, ExpenseType expenseType);
        Task<ExpenseType> Delete(string UID, int id);
    }
}
