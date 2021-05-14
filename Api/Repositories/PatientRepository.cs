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
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientDbContext _context;

        public PatientRepository(PatientDbContext context)
        {
            _context = context;
        }
        public async Task<Patient> Add(Patient patient)
        {
            patient.CreatedAt = DateTime.Now;
            Console.WriteLine(patient.CreatedAt);
            _context.Patients.Add(patient);

            await _context.SaveChangesAsync();

            return  patient;
        }

        public async Task<Patient> Delete(string UID, int id)
        {
            var patient = await _context.Patients.Where(p => p.UserId == UID && p.PatientId == id).FirstAsync();
            if (patient == null)
            {
                return null;
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return patient;
        }

        public async Task<Patient> Get(string UID,int id)
        {
            var patient = await _context.Patients.Where(p=>p.UserId==UID && p.PatientId ==id).FirstAsync();
            return patient;
        }

        public async Task<List<PatientViewModel>> GetAll(string UID)
        {
            var patients = await _context.Patients.Where(p=>p.UserId == UID).Include(p => p.Treatments)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            var result = new List<PatientViewModel>();
            foreach (var patient in patients)
            {
                var totalCost = patient.Treatments.Sum(t => t.TreatmentCost);
                result.Add(new PatientViewModel()
                {
                    PatientId = patient.PatientId,
                    UserId = patient.UserId,
                    Age = patient.Age,
                    Gender = patient.Gender,
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    PhoneNumber = patient.PhoneNumber,
                    TotalTreatmentCost = totalCost,
                    CreatedAt = patient.CreatedAt
                });
            }
            return  result;
        }
        public async Task<List<AdvanceReportViewModel>> GetAllWithTreatments(string UID)
        {
            return await (from patient in _context.Patients
                            join t in _context.Treatments on patient.PatientId equals t.PatientId
                            join ty in _context.TreatmentTypes on t.TreatmentTypeId equals ty.TreatmentTypeId
                            where patient.UserId.Contains(UID)
                            select new AdvanceReportViewModel()
                            {
                                ReportKind = "Patient",
                                TypeOfReportKind = ty.Name,
                                Date = t.CreatedAt,
                                Balance = t.TreatmentCost
                            }
                            ).ToListAsync();
   
                
        }

     

        public async Task<List<FivePatientViewModel>> GetLastFivePatients(string UID)
        {
            return await (from p in _context.Patients
                          where p.UserId.Contains(UID)
                          orderby p.CreatedAt descending
                          select new FivePatientViewModel() {
                              FullName = p.FirstName + " " + p.LastName ,
                              Age = p.Age,
                              Gender = p.Gender,
                              PhoneNumber = p.PhoneNumber,
                              CreatedAt =p.CreatedAt
                          } ).Take(5).ToListAsync();
        }

        public async Task<decimal> GetNewWeeklyPatientsCount(string UID)
        {
            var lastWeek = DateTime.Now.AddDays(-7);
            return await _context.Patients
                                 .Where (p =>p.UserId == UID
                                 &&
                                 DateTime.Compare(p.CreatedAt, lastWeek) >= 0).CountAsync();
        }

        public async Task<decimal> GetPatientsCount(string UID)
        {
            return await _context.Patients.Where(p => p.UserId == UID).CountAsync();
        }

        public async Task<decimal> GetTotalIncomes(string UID)
        {
           var treatments= await _context.Treatments.ToListAsync();
           return treatments.Where(t => t.UserId == UID).Sum(tre => tre.TreatmentCost);
        }

        public async Task<PatientViewModel> Update(int id, Patient patient)
        {
            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            var patientResult = await _context.Patients.Where(p => p.PatientId.Equals(id) && p.UserId.Contains(patient.UserId)).Include(p => p.Treatments)
              .FirstOrDefaultAsync();

            var totalCost = patientResult.Treatments.Sum(t => t.TreatmentCost);
            PatientViewModel newPatient = new PatientViewModel()
            {
                PatientId = patientResult.PatientId,
                UserId = patientResult.UserId,
                Age = patientResult.Age,
                Gender = patientResult.Gender,
                FirstName = patientResult.FirstName,
                LastName = patientResult.LastName,
                PhoneNumber = patientResult.PhoneNumber,
                TotalTreatmentCost = totalCost,
                CreatedAt = patientResult.CreatedAt
            };

            return newPatient;
        }
        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientId == id);
        }
    }
}
