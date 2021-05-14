using Api.Data;
using Api.Models;
using Api.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly IPatientRepository _patientRepository;
        private readonly ExpenseDbContext _expenseContext;

        public ReportRepository (IPatientRepository patientRepository , ExpenseDbContext expenseContext)
        {
            _patientRepository = patientRepository;
            _expenseContext = expenseContext;
        }

        

        public async Task<List<ReportViewModel>> GetAllReports(string UID)
        {
            var patients = await _patientRepository.GetAll(UID);
            var expenses = await _expenseContext.Expenses.Where(e=>e.UserId == UID).ToListAsync();


            //  Console.WriteLine();
            //  patients = patients.OrederBy(p => p.CreatedAt).ToList();

            List<ReportViewModel> patientReports = patients.Select(p => new ReportViewModel() { TypeOfReport = "Patient", Date = p.CreatedAt, Balance = p.TotalTreatmentCost }).ToList();
            List<ReportViewModel> expenseReports = expenses.Select(e => new ReportViewModel() { TypeOfReport = "Expense", Date = e.CreatedAt, Balance = -e.ExpenseValue }).ToList();
            patientReports.AddRange(expenseReports);
            patientReports = patientReports.OrderByDescending(pe => pe.Date).ToList();
            foreach (var report in patientReports)
             {
                 Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(report));
             }

            return patientReports;

        }
        public async Task<List<AdvanceReportViewModel>> GetAllAdvanceReports(string UID)
        {
            List<AdvanceReportViewModel> patientReports = await _patientRepository.GetAllWithTreatments(UID);
            var expenses = await _expenseContext.Expenses.Where(e=>e.UserId==UID).Include(e => e.ExpenseType).ToListAsync();
            List<AdvanceReportViewModel> expenseReports = 
                expenses.Select(e => 
                new AdvanceReportViewModel() 
                { ReportKind = "Expense", 
                    TypeOfReportKind = e.ExpenseType.ExpenseTypeName, 
                    Date = e.CreatedAt, 
                    Balance = -e.ExpenseValue 
                }).ToList();
            patientReports.AddRange(expenseReports);
            patientReports = patientReports.OrderByDescending(pe => pe.Date).ToList();
            foreach (var report in patientReports)
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(report));
            }

            return patientReports;
        }
    }
}
