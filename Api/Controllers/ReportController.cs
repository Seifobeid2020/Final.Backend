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
    public class ReportController : Controller
    {

        private readonly IReportRepository _repository;
        private readonly FirebaseAuth auth;
        public ReportController(IReportRepository repository)
        {
            _repository = repository;
            auth = FirebaseAuth.GetAuth(FirebaseApp.DefaultInstance);
        }

        // GET: api/Report
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportViewModel>>> GetReports()
        {


            var reports = await _repository.GetAllReports(getUID().Result.ToString());
            return Ok(reports);
        }

        // GET: api/advanceReport
        [HttpGet("advanceReport")]
        public async Task<ActionResult<IEnumerable<ReportViewModel>>> GetAdavanceReports()
        {

            var advanceReports = await _repository.GetAllAdvanceReports(getUID().Result.ToString());
            return Ok(advanceReports);
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
