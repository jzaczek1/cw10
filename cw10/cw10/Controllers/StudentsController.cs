using System;
using cw10.DTOs;
using cw10.Models;
using cw10.Services;
using Microsoft.AspNetCore.Mvc;

namespace cw10.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {

        private IStudentDbService myService;
        public StudentsController(IStudentDbService service)
        {
            myService = service;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok(myService.GetStudents());
        }

        [HttpDelete]
        public IActionResult DeleteStudent(DeleteStudentRequest request)
        {
            try
            {
                Student student = myService.DeleteStudent(request);
                return Ok("Student " + student + "deleted");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult ModifyStudent(ModifyStudentRequest request)
        {
            try
            {
                Student student = myService.ModifyStudent(request);
                return Ok("Student " + student + "modified");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}