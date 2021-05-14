using Api.Models;
using Api.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Repositories
{
   public interface IExpenseRepository
    {
        Task<List<Expense>> GetAll(string UID);
        Task<Expense> Get(string UID, int id);
        Task<Expense> Add(Expense expense);
        Task<Expense> Update(int id, Expense expense);
        Task<Expense> Delete(string UID,int id);
        Task<decimal> GetTotalExpenses(string UID);
        Task<List<FiveExpenseViewModel>> GetLastFiveExpenses(string UID);

    }
}
