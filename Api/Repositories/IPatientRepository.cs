using Api.Models;
using Api.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Repositories
{
    public interface IPatientRepository
    {
        Task<List<PatientViewModel>> GetAll(string UID);
        Task<List<AdvanceReportViewModel>> GetAllWithTreatments(string UID);
        Task<Patient> Get(string UID,int id);
        Task<Patient> Add(Patient patient);
        Task<PatientViewModel> Update(int id, Patient patient);
        Task<Patient> Delete(string UID, int id);
        Task<decimal> GetPatientsCount(string UID);
        Task<decimal> GetTotalIncomes(string UID);
        Task<decimal> GetNewWeeklyPatientsCount(string UID);
        Task<List<FivePatientViewModel>> GetLastFivePatients(string UID);

     

    }
}
