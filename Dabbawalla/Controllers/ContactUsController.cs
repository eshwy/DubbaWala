using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dabbawalla.Models;
using Dabbawalla.Services;

namespace Dabbawalla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly EmailListenerService _emailListenerService;

        public ContactUsController(MyDbContext context, EmailListenerService emailListenerService)
        {
            _context = context;
            _emailListenerService = emailListenerService;
        }

        // GET: api/ContactUs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactUs>>> GetContactUs()
        {
            return await _context.ContactUs.ToListAsync();
        }

        
        // POST: api/ContactUs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ContactUs>> PostContactUs(ContactUs contactUs)
        {
            _context.ContactUs.Add(contactUs);
            await _context.SaveChangesAsync();

            var admins = _context.Users.Where(x => x.RoleId == 1).Select(x=>x.EmailAddress).ToList();
            var id = contactUs.Id;
            _emailListenerService.SendAdminMailDForContact(admins, contactUs, CancellationToken.None);
            return CreatedAtAction("GetContactUs", new { id=id }, contactUs);
        }
    }
}
