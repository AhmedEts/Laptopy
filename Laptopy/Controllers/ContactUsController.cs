using Laptopy.DTOs.Request;
using Laptopy.Models;
using Laptopy.Repositories.IRepositories;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Laptopy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]



    public class ContactUsController : ControllerBase
    {
        private readonly IContactUsRepository _usRepository;
        public ContactUsController(IContactUsRepository usRepository
            )
        {
            this._usRepository = usRepository;
        }
        [HttpGet("")]
        public IActionResult GetAll()
        {
            var contacts = _usRepository.Get();
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public IActionResult GetOne([FromRoute] int id)
        {
            var contact = _usRepository.GetOne(e => e.Id == id);
            if (contact != null)
            {
                return Ok(contact);
            }
            return NotFound();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var contact = _usRepository.GetOne(e => e.Id == id);
            if (contact != null)
            {
                _usRepository.Delete(contact);
                return NoContent();
            }
            return NotFound();
        }


        [HttpPut("{id}")]
        public IActionResult Edit([FromRoute] int id, ContactUsRequest contactus)
        {

            var contact = _usRepository.GetOne(e => e.Id == id);
            if (contact != null)
            {

                contact.Name = contactus.Name;
                contact.Email = contactus.Email;
                contact.Message = contactus.Message;
                contact.Subject = contactus.Subject;

                _usRepository.Edit(contact);
                return NoContent();

            }
            return NotFound();
        }

        [HttpPost("")]
        public IActionResult Create(ContactUsRequest contactUs)
        {

            var contact = _usRepository.Create(new Models.ContactUs
            {
                Name = contactUs.Name,
                Email = contactUs.Email,
                Message = contactUs.Message,
                Subject = contactUs.Subject
                //ApplicationUserId=contactUs.ApplicationUserId

            });
            return Ok();

        }
    }
}
