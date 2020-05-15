using cw10.DTOs;
using cw10.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace cw10.Services
{
    public class SqlStudentDbService : IStudentDbService
    {
        public s18593Context myContext { get; set; }

        public SqlStudentDbService(s18593Context context)
        {
            myContext = context;
        }


        public IEnumerable GetStudents()
        {
            var list = myContext.Student.ToList();
            return list;
        }

        public Student DeleteStudent(DeleteStudentRequest request)
        {
            var stud = myContext.Student.First(x => x.IndexNumber == request.IndexNumber);
            if (stud != null)
            {
                myContext.Remove(stud);
                myContext.SaveChanges();
            }
            else
            {
                throw new Exception("There is no such a student");
            }
            return stud;
        }

        public Student ModifyStudent(ModifyStudentRequest request)
        {
            var student = myContext.Student.FirstOrDefault(e => e.IndexNumber == request.IndexNumber);

            if (student == null)
                throw new Exception("There is no such a student");

            student.IndexNumber = request.IndexNumber;
            student.FirstName = request.FirstName;
            student.LastName = request.LastName;
            student.BirthDate = request.BirthDate;
            student.IdEnrollment = request.IdEnrollment;

            myContext.SaveChanges();

            return student;
        }

        public Enrollment EnrollStudent(EnrollStudentRequest request)
        {
            var studies = myContext.Studies.FirstOrDefault(s => s.Name == request.StudiesName);

            if (studies == null)
                throw new Exception("There is no such a studies");


            var enrollment = myContext.Enrollment.Where(e => e.IdStudy == studies.IdStudy && e.Semester == 1)
                .OrderByDescending(e => e.StartDate).FirstOrDefault();

            if (enrollment == null)
            {
                enrollment = new Enrollment()
                {
                    IdEnrollment = myContext.Enrollment.Max(e => e.IdEnrollment) + 1,
                    Semester = 1,
                    IdStudy = studies.IdStudy,
                    StartDate = DateTime.Now
                };
                myContext.Enrollment.Add(enrollment);
            }

            var czyStudentIstnieje = myContext.Student.FirstOrDefault(e => e.IndexNumber == request.IndexNumber);

            if (czyStudentIstnieje != null)
                throw new Exception("Try another student");


            var student = new Student()
            {
                IndexNumber = request.IndexNumber,
                BirthDate = Convert.ToDateTime(request.BirthDate),
                FirstName = request.FirstName,
                LastName = request.LastName,
                IdEnrollment = enrollment.IdEnrollment
            };

            myContext.Student.Add(student);
            myContext.SaveChanges();
            return enrollment;

        }

        public Enrollment PromoteStudent(PromoteStudentRequest request)
        {
            var res = myContext.Enrollment.Join(myContext.Studies, enroll => enroll.IdStudy, stud => stud.IdStudy,
                (enroll, stud) => new
                {
                    stud.Name,
                    enroll.Semester
                }).FirstOrDefault(e => e.Name == request.Studies && e.Semester == request.Semester);

            if (res == null)
                throw new Exception("There is no in Enrollment");

            myContext.Database.ExecuteSqlRaw("EXEC PromoteStudents @Studies, @Semester", request.Studies, request.Semester);
            myContext.SaveChanges();

            var res2 = myContext.Enrollment.Join(myContext.Studies, enroll => enroll.IdStudy, stud => stud.IdStudy,
                (enroll, stud) => new
                {
                    enroll.IdEnrollment,
                    enroll.Semester,
                    enroll.IdStudy,
                    enroll.StartDate,
                    stud.Name
                }).Where(e => e.Name == request.Studies && e.Semester == request.Semester + 1).ToList();

            var enrollment = new Enrollment()
            {
                IdEnrollment = res2[0].IdEnrollment,
                Semester = res2[0].Semester,
                IdStudy = res2[0].IdStudy,
                StartDate = res2[0].StartDate
            };
            return enrollment;
        }
    }
}