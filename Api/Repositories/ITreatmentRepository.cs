using Api.Models;
using Api.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Repositories
{
    public interface ITreatmentRepository
    {
        Task<List<TreatmentViewModel>> GetAll(string UID, int patientId);
        Task<Treatment> Get(string UID, int treatmentId);
        Task<TreatmentViewModel> Add(Treatment treatment);
        Task<TreatmentViewModel> Update(int id, Treatment treatment);
        Task<Treatment> Delete(string UID, int id);
    }
}
