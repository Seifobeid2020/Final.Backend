using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Repositories
{
    public interface ITreatmentTypeRepository
    {
        Task<List<TreatmentType>> GetAll(string UID);
        Task<TreatmentType> Get(string UID, int id);
        Task<TreatmentType> Add(TreatmentType treatmentType);
        Task<TreatmentType> Update(int id, TreatmentType treatmentType);
        Task<TreatmentType> Delete(string UID, int id);
    }
}
