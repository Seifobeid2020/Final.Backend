using Api.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Repositories
{
    public interface IDashboardRepository
    {
        Task<decimal> PatientsCount(string UID);
        Task<decimal> TotalExpanse(string UID);
        Task<decimal> TotalIncomes(string UID);
        Task<decimal> NewWeeklyPatientsCount(string UID);
        Task<List<FivePatientViewModel>> LastFivePatients(string UID);
        Task<List<FiveExpenseViewModel>> LastFiveExpenses(string UID);


    }
}
