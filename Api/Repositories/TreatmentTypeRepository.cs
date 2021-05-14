using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Repositories
{
    public class TreatmentTypeRepository : ITreatmentTypeRepository
    {
        private readonly PatientDbContext _context;
        public TreatmentTypeRepository(PatientDbContext context)
        {
            _context = context;
        }
        public async Task<TreatmentType> Add(TreatmentType treatmentType)
        {
           var result= _context.TreatmentTypes.Add(treatmentType);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<TreatmentType> Delete(string UID, int id)
        {
            var treatmentType = await _context.TreatmentTypes.Where(tt=>tt.UserId.Contains(UID)&& tt.TreatmentTypeId.Equals(id)).FirstAsync();
            if (treatmentType == null)
            {
                return null;
            }

            _context.TreatmentTypes.Remove(treatmentType);
            await _context.SaveChangesAsync();
            return treatmentType ;
        }

        public async Task<TreatmentType> Get(string UID, int id)
        {
            var treatmentType = await _context.TreatmentTypes.Where(tt=>tt.UserId.Contains(UID) && tt.TreatmentTypeId.Equals(id)).FirstAsync();
            return treatmentType;
        }

        public async Task<List<TreatmentType>> GetAll(string UID)
        {
            return await _context.TreatmentTypes.Where(tt=>tt.UserId.Contains(UID)).ToListAsync();
        }

        public async Task<TreatmentType> Update(int id, TreatmentType treatmentType)
        {
            _context.Entry(treatmentType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TreatmentTypeExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return treatmentType;
        }

        private bool TreatmentTypeExists(int id)
        {
            return _context.TreatmentTypes.Any(e => e.TreatmentTypeId == id);
        }
    }


}
