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
    public class TreatmentRepository : ITreatmentRepository
    {
        private readonly PatientDbContext _context;
        public TreatmentRepository(PatientDbContext context)
        {
            _context = context;
        }
        public async Task<TreatmentViewModel> Add(Treatment treatment)
        {
            treatment.CreatedAt = DateTime.Now;
            _context.Treatments.Add(treatment);
            await _context.SaveChangesAsync();

            var treatmentType = await _context.TreatmentTypes.FindAsync(treatment.TreatmentTypeId);


            var result = new TreatmentViewModel()
            {
                TreatmentId = treatment.TreatmentId,
                UserId = treatment.UserId,
                TreatmentCost = treatment.TreatmentCost,
                CreatedAt = treatment.CreatedAt,
                TreatmentImageUrl = treatment.TreatmentImageUrl,
                TreatmentImageName = treatment.TreatmentImageName,
                PatientId = treatment.PatientId,
                TreatmentName = treatmentType.Name,
                TreatmentTypeId = treatment.TreatmentTypeId
            };

            return  result;

        }

        public async Task<Treatment> Delete(string UID, int treatmentId)
        {
            var treatment = await _context.Treatments.Where(treatment => treatment.UserId.Contains(UID) && treatment.TreatmentId.Equals(treatmentId))
                            .FirstAsync();
                if (treatment == null)
            {
                return null;
            }

           var tre= _context.Treatments.Remove(treatment);
            await _context.SaveChangesAsync();
            return  tre.Entity;
        }

        public async Task<Treatment> Get(string UID, int treatmentId)
        {

            var treatment = await _context.Treatments
                .Where(treatment=>  treatment.TreatmentId== treatmentId)//treatment.UserId.Contains(UID)&&
                .FirstOrDefaultAsync();

            Console.WriteLine(treatment);
            return treatment;

        }

        public async Task<List<TreatmentViewModel>> GetAll(string UID,int patientId )
        {
            var treatments = await _context.Treatments
                .Where(treatment => treatment.UserId.Contains(UID) && treatment.PatientId == patientId)
                .Include(p => p.TreatmentType)
                .ToListAsync();

            var result = new List<TreatmentViewModel>();
            foreach (var treatment in treatments)
            {

                result.Add(new TreatmentViewModel()
                {
                    TreatmentId = treatment.TreatmentId,
                    UserId = treatment.UserId,
                    TreatmentCost = treatment.TreatmentCost,
                    CreatedAt = treatment.CreatedAt,
                    TreatmentImageUrl = treatment.TreatmentImageUrl,
                    TreatmentImageName = treatment.TreatmentImageName,
                    PatientId = treatment.PatientId,
                    TreatmentName = treatment.TreatmentType.Name,
                    TreatmentTypeId = treatment.TreatmentTypeId
                });
            }
            return result;
        }

        public async Task<TreatmentViewModel> Update(int id, Treatment treatment)
        {
           

            _context.Entry(treatment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TreatmentExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            var treatmentType = await _context.TreatmentTypes.FindAsync(treatment.TreatmentTypeId);
            var t = await _context.Treatments.Where(t=>t.UserId.Contains(treatment.UserId)&& t.TreatmentId.Equals(id)).FirstAsync();


            var result = new TreatmentViewModel()
            {
                TreatmentId = t.TreatmentId,
                UserId = t.UserId,
                TreatmentCost = t.TreatmentCost,
                CreatedAt = t.CreatedAt,
                TreatmentImageUrl = t.TreatmentImageUrl,
                TreatmentImageName = t.TreatmentImageName,
                PatientId = t.PatientId,
                TreatmentName = treatmentType.Name,
                TreatmentTypeId = t.TreatmentTypeId
            };

            return result;
        }

        private bool TreatmentExists(int id)
        {
            return _context.Treatments.Any(e => e.TreatmentId == id);
        }
    }
}
