using DigiturkFilmAPI.Domain;
using DigiturkFilmAPI.Models;
using DigiturkFilmAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigiturkFilmAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmController : ControllerBase
    {
        private readonly FilmService _filmService;

        public FilmController(FilmService filmService)
        {
            _filmService = filmService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<Film>> GetAll()
        {
            return Ok(_filmService.GetAll());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<Film> GetById(string id)
        {
            return Ok(_filmService.GetById(id));
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<Film> Create(FilmRequest request)
        {
            return Ok(_filmService.Create(request));
        }

        [HttpPatch]
        [AllowAnonymous]
        public ActionResult<Film> Update(Film request)
        {
            return Ok(_filmService.Patch(request));
        }

        [HttpDelete]
        [AllowAnonymous]
        public ActionResult<bool> Delete(string id)
        {
            return Ok(_filmService.Delete(id));
        }
    }
}
