using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayInn.Application.DTOs.Dashboard;
using StayInn.Application.Interfaces.Service;

namespace StayInn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpGet("resumen")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<DashboardDto>> GetResumen()
        {
            var datos = await _service.ObtenerResumenAsync();
            return Ok(datos);
        }
    }
}
