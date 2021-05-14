using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Models;
using FirebaseAdmin.Auth;
using FirebaseAdmin;
using Api.Repositories;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreatmentTypesController : ControllerBase
    {
        /*        private readonly PatientDbContext _context;
        */
        private readonly ITreatmentTypeRepository _repository;
        private readonly FirebaseAuth auth;
        public TreatmentTypesController(ITreatmentTypeRepository repository)
        {
            _repository = repository;
            auth = FirebaseAuth.GetAuth(FirebaseApp.DefaultInstance);
        }

        // GET: api/TreatmentTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TreatmentType>>> GetTreatmentTypes()
        {

            var result = await _repository.GetAll(getUID().Result);

            return  result;
        }

        // GET: api/TreatmentTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TreatmentType>> GetTreatmentType(int id)
        {
            /*  var treatmentType = await _context.TreatmentTypes.FindAsync(id);*/
            var treatmentType = await _repository.Get(getUID().Result, id);

            if (treatmentType == null)
            {
                return NotFound();
            }

            return treatmentType;
        }

        // PUT: api/TreatmentTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTreatmentType(int id, TreatmentType treatmentType)
        {
            if (id != treatmentType.TreatmentTypeId)
            {
                return BadRequest();
            }

            treatmentType.UserId = getUID().Result;
            var result = await _repository.Update(id, treatmentType);

            if (result==null)
            {
                return NotFound();
            }
          

            return  CreatedAtAction("GetTreatmentType", new { id = treatmentType.TreatmentTypeId }, treatmentType);
        }

        // POST: api/TreatmentTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TreatmentType>> PostTreatmentType(TreatmentType treatmentType)
        {
            treatmentType.UserId = getUID().Result;
           var result= await  _repository.Add(treatmentType);

            return CreatedAtAction("GetTreatmentType", new { id = result.TreatmentTypeId }, result);
        }

        // DELETE: api/TreatmentTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTreatmentType(int id)
        {
            var treatmentType = await _repository.Delete(getUID().Result, id);
            if (treatmentType == null)
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
