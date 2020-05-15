using cw10.DTOs;
using cw10.Models;
using System.Collections;


namespace cw10.Services
{
    public interface IStudentDbService
    {
        public IEnumerable GetStudents();
        public Student ModifyStudent(ModifyStudentRequest request);
        public Student DeleteStudent(DeleteStudentRequest request);

        Enrollment EnrollStudent(EnrollStudentRequest request);
        Enrollment PromoteStudent(PromoteStudentRequest request);
    }
}