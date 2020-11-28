using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Avalentini.Expensi.Api.Contracts.Models;
using Avalentini.Expensi.Core.Data.Entities;
using Avalentini.Expensi.Api.Data.Repository.Expenses;
using MongoDB.Driver;

namespace Avalentini.Expensi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly ExpensesMongoRepository _repo;

        public ExpensesController(IMongoCollection<UserEntity> collection, IMapper mapper)
        {
            _repo = new ExpensesMongoRepository(collection, mapper);
        }

        // GET: api/Expenses
        [HttpGet]
        public async Task<IEnumerable<Expense>> Get(int userId)
        {
            return await _repo.GetAll(userId).ConfigureAwait(false);
        }
    }
}
