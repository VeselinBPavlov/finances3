using Finances.Services.CashflowTypes.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace Finances.Controllers
{
    public class CashflowTypeController : ApiController
    {
        //GET: api/CashflowType
        [HttpGet]
        public async Task<ActionResult<List<CashflowTypesVm>>> GetAll()
        {
            var result = await Mediator.Send(new GetlCashflowTypesListQuery());

            return Ok(result);
        }
    }
}
