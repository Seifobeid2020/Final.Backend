using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Models;
using Api.Models.ViewModels;
using FirebaseAdmin.Auth;
using FirebaseAdmin;
using Api.Repositories;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreatmentsController : ControllerBase
    {
       
        private readonly ITreatmentRepository _repository;
        private readonly FirebaseAuth auth;
        public TreatmentsController(ITreatmentRepository repository)
        {
            _repository = repository;
            auth = FirebaseAuth.GetAuth(FirebaseApp.DefaultInstance);
        }

        /*// GET: api/Treatments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Treatment>>> GetTreatments()
        {
            return await _context.Treatments.ToListAsync();
        }*/
        // GET: api/Treatments/patient/5
        [HttpGet("patient/{id}")]
        public async Task<ActionResult<IEnumerable<Treatment>>> GetTreatmentsByPatientId(int id)
        {
            var result = await _repository.GetAll(getUID().Result, id);

            return Ok(result);
        }

        // GET: api/Treatments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Treatment>> GetTreatment(int treatmentId)
        {
            /*        var treatment = await _context.Treatments.FindAsync(id);*/
            var treatment = await _repository.Get(getUID().Result, treatmentId);

            if (treatment == null)
            {
                return NotFound();
            }

            return Ok(treatment);
        }

        // PUT: api/Treatments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<TreatmentViewModel>> PutTreatment(int id, Treatment treatment)
        {
            if (id != treatment.TreatmentId)
            {
               
                return BadRequest();
            }

            treatment.UserId = getUID().Result;
            var result = await _repository.Update(id, treatment);
                if (result == null)
                {
                    return NotFound();
                }
  
            return result;
        }

        // POST: api/Treatments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TreatmentViewModel>> PostTreatment(Treatment treatment)
        {
            treatment.UserId = getUID().Result;
            var result = await _repository.Add(treatment);

            return  Ok(result);
        }

        // DELETE: api/Treatments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTreatment(int id)
        {
            var treatment = await _repository.Delete(getUID().Result, id);
            if (treatment == null)
            {
                return NotFound();
            }

          

            return NoContent();
        }

       
        private async Task<string> getUID()
        {
            var idToken = HttpContext.Request.Headers["Authorization"].ToString();
            idToken = idToken.Split("key ")[1];
            FirebaseToken decodedToken = await auth.VerifyIdTokenAsync(idToken);
            return decodedToken.Uid;

        }

    }
}