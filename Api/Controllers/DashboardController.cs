using Api.Models.ViewModels;
using Api.Repositories;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly FirebaseAuth auth;
       public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;

            auth = FirebaseAuth.GetAuth(FirebaseApp.DefaultInstance);
        }

        // GET: api/PatientCount
        [HttpGet("PatientCount")]
        public async Task<ActionResult<decimal>> GetPatientsCount()
        {

            
            return await _dashboardRepository.PatientsCount(getUID().Result.ToString());
        }

        // GET: api/LastFivePatients
        [HttpGet("LastFivePatients")]
        public async Task<ActionResult<List<FivePatientViewModel>>> GetLastFivePatients()
        {
            return  Ok( await _dashboardRepository.LastFivePatients(getUID().Result));
        }
        // GET: api/TotalExpanse
        [HttpGet("TotalExpanse")]
        public async Task<ActionResult<decimal>> GetTotalExpanse()
        {
           
            return await _dashboardRepository.TotalExpanse(getUID().Result);
        }

        // GET: api/TotalIncomes
        [HttpGet("TotalIncomes")]
        public async Task<ActionResult<decimal>> GetTotalIncomes()
        {
            return await _dashboardRepository.TotalIncomes(getUID().Result);
        }
        // GET: api/NewWeeklyPatientsCount
        [HttpGet("NewWeeklyPatientsCount")]
        public async Task<ActionResult<decimal>> GetNewWeeklyPatientsCount()
        {
            return await _dashboardRepository.NewWeeklyPatientsCount(getUID().Result);
        }

        // GET: api/LastFiveExpenses
        [HttpGet("LastFiveExpenses")]
        public async Task<ActionResult<List<FiveExpenseViewModel>>> GetLastFiveExpenses()
        {
            return await _dashboardRepository.LastFiveExpenses(getUID().Result);
        }

        private  async Task<string> getUID()
        {
            var idToken = HttpContext.Request.Headers["Authorization"].ToString();
            idToken = idToken.Split("key ")[1];
            FirebaseToken decodedToken = await auth.VerifyIdTokenAsync(idToken);
            return decodedToken.Uid;

        }
    }
}
