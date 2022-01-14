using System;
using Microsoft.AspNetCore.Mvc;
using ShowAPI.Interfaces;
using ShowAPI.Models;

namespace ShowAPI.Controllers
{
    [ApiController]
    [Route("api/shows")]
    public class ShowController : Controller
    {
        //Interface works only for this subpage
        private readonly IShowService _showService;

        //Makes showservice showservice(idk man, ask soni)
        public ShowController(IShowService showService)
        {
            _showService = showService;
        }

        //What to do when httpget
        [HttpGet]
        public Show GetShow(int id)
        {
            return _showService.GetShow(id);
        }
        
        //What to do when httpget also adds a /all
        [HttpGet("all")]
        public IEnumerable<Show> GetAllShows()
        {
            return _showService.GetAllShows();
        }
    }
}