using Api.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IExpenseRepository _expenseRepository;

        public DashboardRepository(IPatientRepository patientRepository, IExpenseRepository expenseRepository) {
            _patientRepository = patientRepository;
            _expenseRepository = expenseRepository;
        }
        public async Task<List<FiveExpenseViewModel>> LastFiveExpenses(string UID)
        {
            return await _expenseRepository.GetLastFiveExpenses( UID);
        }

        public async Task<List<FivePatientViewModel>> LastFivePatients(string UID)
        {
            return await _patientRepository.GetLastFivePatients(UID);
        }

        public async Task<decimal> NewWeeklyPatientsCount(string UID)
        {
            return await _patientRepository.GetNewWeeklyPatientsCount(UID);
        }

        public async Task<decimal> PatientsCount(string UID)
        {
            return await _patientRepository.GetPatientsCount(UID);
        }

        public async Task<decimal> TotalExpanse(string UID)
        {
            return await _expenseRepository.GetTotalExpenses(UID);        }

        public async Task<decimal> TotalIncomes(string UID)
        {
            return await _patientRepository.GetTotalIncomes(UID);
        }
    }
}
