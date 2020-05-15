using System;
using cw10.DTOs;
using cw10.Models;
using cw10.Services;
using Microsoft.AspNetCore.Mvc;

namespace cw10.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentDbService myService;

        public EnrollmentsController(IStudentDbService service)
        {
            myService = service;
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            try
            {
                Enrollment enrollment = myService.EnrollStudent(request);
                return Ok(enrollment);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("promotions")]
        public IActionResult PromoteStudent(PromoteStudentRequest request)
        {
            try
            {
                Enrollment enrollment = myService.PromoteStudent(request);
                return Ok(enrollment);
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

    }
}