using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Avalentini.Expensi.Api.Contracts.Models;
using Avalentini.Expensi.Api.Data.Entities;
using Avalentini.Expensi.Api.Data.Repository.Expenses;
using MongoDB.Driver;

namespace Avalentini.Expensi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private ExpensesMongoRepository _repo;
        private readonly IMongoCollection<ExpensesPerUser> _collection;
        private readonly IMapper _mapper;

        public ExpensesMongoRepository Repo
        {
            get
            {
                if (_repo != null)
                    return _repo;

                //var userId = int.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
                const int userId = 2;
                _repo = new ExpensesMongoRepository(_collection, _mapper, userId);

                return _repo;
            }
        }

        public ExpensesController(IMongoCollection<ExpensesPerUser> collection, IMapper mapper)
        {
            _collection = collection;
            _mapper = mapper;
        }

        // GET: api/Expenses
        [HttpGet]
        public async Task<IEnumerable<Expense>> Get()
        {
            return await Repo.GetAll().ConfigureAwait(false);
        }
    }
}
