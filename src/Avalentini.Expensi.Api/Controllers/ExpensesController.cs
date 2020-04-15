using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using Avalentini.Expensi.Api.Contracts.Models;
using Avalentini.Expensi.Core.Data.Entities;
using Avalentini.Expensi.Api.Data.Repository.Expenses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace Avalentini.Expensi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("userIdPolicy")]
    public class ExpensesController : ControllerBase
    {
        private readonly ExpensesMongoRepository _repo;

        public ExpensesController(IMongoCollection<ExpensesPerUser> collection, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null) throw new ArgumentNullException(nameof(httpContextAccessor));
            var claim = httpContextAccessor.HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2020/04/identity/claims/userid");
            _repo = CreateRepo(collection, mapper, claim);
        }

        private ExpensesMongoRepository CreateRepo(IMongoCollection<ExpensesPerUser> collection, IMapper mapper, System.Security.Claims.Claim claim)
        {
            if (claim == null) throw new ArgumentNullException(nameof(claim));
            var userId = int.Parse(claim.Value, NumberStyles.Integer, CultureInfo.CurrentCulture);
            return new ExpensesMongoRepository(collection, mapper, userId);
        }

        // GET: api/Expenses
        [HttpGet]
        public async Task<IEnumerable<Expense>> Get()
        {
            return await _repo.GetAll().ConfigureAwait(false);
        }
    }
}
