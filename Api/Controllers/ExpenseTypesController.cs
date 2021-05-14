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
    public class ExpenseTypesController : ControllerBase
    {
/*        private readonly ExpenseDbContext _context;*/
        private readonly IExpenseTypeRepository _repository;
        private readonly FirebaseAuth auth;
        public ExpenseTypesController(IExpenseTypeRepository repository)
        {
            _repository = repository;
            auth = FirebaseAuth.GetAuth(FirebaseApp.DefaultInstance);
        }

        // GET: api/ExpenseTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseType>>> GetExpenseTypes()
        {
            var resutl = await _repository.GetAll(getUID().Result);
            Console.WriteLine(resutl);
            return resutl;
        }

        // GET: api/ExpenseTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseType>> GetExpenseType(int id)
        {
            var expenseType = await _repository.Get(getUID().Result,id);

            if (expenseType == null)
            {
                return NotFound();
            }

            return expenseType;
        }

        // PUT: api/ExpenseTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpenseType(int id, ExpenseType expenseType)
        {
           if (id != expenseType.ExpenseTypeId)
            {
                return BadRequest();
            }

            var result = _repository.Update(id, expenseType);
            if (result.Result == null)
            {
                return NotFound();
            }
            return CreatedAtAction("GetExpenseType", new { id = result.Result.ExpenseTypeId }, result.Result);
        }

        // POST: api/ExpenseTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExpenseType>> PostExpenseType(ExpenseType expenseType)
        {
            /*  _context.ExpenseTypes.Add(expenseType);
              await _context.SaveChangesAsync();*/

            expenseType.UserId = await getUID();
            var result =await  _repository.Add(expenseType);

            return CreatedAtAction("GetExpenseType", new { id = result.ExpenseTypeId }, result);
        }

        // DELETE: api/ExpenseTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenseType(int id)
        {
            /* var expenseType = await _context.ExpenseTypes.FindAsync(id);
             if (expenseType == null)
             {
                 return NotFound();
             }

             _context.ExpenseTypes.Remove(expenseType);
             await _context.SaveChangesAsync();*/

            var result = await _repository.Delete(getUID().Result, id);

            if (result == null)
            {
                return NotFound();
            }
            Console.WriteLine(result);
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
