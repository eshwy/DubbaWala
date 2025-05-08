using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dabbawalla.Models;

namespace Dabbawalla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedBacksController : ControllerBase
    {
        private readonly MyDbContext _context;

        public FeedBacksController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/FeedBacks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedBack>>> GetFeedBack()
        {
            return await _context.FeedBack.ToListAsync();
        }

        // POST: api/FeedBacks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FeedBack>> PostFeedBack(string feedBack)
        {
            var feedback = new FeedBack
            {
                Messagae= feedBack
            };
            _context.FeedBack.Add(feedback);
            await _context.SaveChangesAsync();

            return Ok("Created Successfully");
        }
    }
}
