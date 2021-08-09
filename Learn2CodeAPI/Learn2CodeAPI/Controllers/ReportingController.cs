﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Learn2CodeAPI.Data;
using Learn2CodeAPI.Dtos.ReportDto;
using Learn2CodeAPI.Models.Login.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learn2CodeAPI.Controllers
{
    [Route("api/Reporting")]
    [ApiController]
    public class ReportingController : ControllerBase
    {
        

            private readonly UserManager<AppUser> _userManager;
            private IMapper _mapper;
            private readonly AppDbContext db;

            public ReportingController(UserManager<AppUser> userManager, IMapper mapper,
                AppDbContext _db)
            {

                _userManager = userManager;
                _mapper = mapper;
                db = _db;

            }

        #region AdminHome
            [HttpGet]
            [Route("TotalStudents")]
            public async Task<IActionResult> TotalStudents()
            {
                int students = await db.Students.CountAsync();
                return Ok(students);
            }

            [HttpGet]
            [Route("TotalTutors")]
            public async Task<IActionResult> TotalTutors()
            {
                int students = await db.Tutor.CountAsync();
                return Ok(students);
            }

            [HttpGet]
            [Route("TotalUniversities")]
            public async Task<IActionResult> TotalUniversities()
            {
                int students = await db.University.CountAsync();
                return Ok(students);
            }
            [HttpGet]
            [Route("TotalDegrees")]
            public async Task<IActionResult> TotalDegrees()
            {
                int students = await db.Degrees.CountAsync();
                return Ok(students);
            }
            [HttpGet]
            [Route("TotalModules")]
            public async Task<IActionResult> TotalModules()
            {
                int students = await db.Degrees.CountAsync();
                return Ok(students);
            }

        #endregion

        #region Tutordetails
        [HttpGet]
        [Route("TutorDetails")]
        public async Task<IActionResult> TutorDetails()
        {
            var Tutors = await db.Tutor.ToListAsync();
            return Ok(Tutors);
        }
        #endregion

        #region studentdetails
        [HttpGet]
        [Route("TutorDetails")]
        public async Task<IActionResult> GetAllStudents()
        {

            var Students = await db.Students.Include(zz => zz.Identity).Include(zz => zz.StudentModule).ThenInclude(StudentModule => StudentModule.Tutor.Degree.University).ToListAsync();
            return Ok(Students);
        }


        #endregion

        #region Attendance
        [HttpGet]
        [Route("AttendacSession")]
        public async Task<IActionResult> AttendacSession()
        {

            var Sessions = await db.BookingInstance.Where(zz => zz.AttendanceTaken == true 
            && zz.TutorSession.SessionType.SessionTypeName == "Group" ).ToListAsync();
            return Ok(Sessions);
        }

        // for table 
        [HttpGet]
        [Route("SessionAttendanceReport/{BookingInstanceId}")]
        public async Task<IActionResult> SessionAttendanceReport(int BookingInstanceId)
        {

            var Attendance = await db.RegisteredStudent.Where(zz => zz.BookingInstanceId == BookingInstanceId).Include(zz => zz.Student).ThenInclude(zz => zz.Identity).ToListAsync();
            return Ok(Attendance);
        }

        //use ng2
        [HttpGet]
        [Route("SessionAttendanceGraph/{BookingInstanceId}")]
        public async Task <IActionResult> SessionAttendanceGraph(int BookingInstanceId)
        {

            var Attendance = await db.RegisteredStudent.Where(zz => zz.BookingInstanceId == BookingInstanceId).ToListAsync();
            int attended = await db.RegisteredStudent.Where(zz => zz.BookingInstanceId == BookingInstanceId && zz.Attended == true).CountAsync();
            int Missed = await db.RegisteredStudent.Where(zz => zz.BookingInstanceId == BookingInstanceId && zz.Attended == false).CountAsync();

            AttendanceGraphDto dto = new AttendanceGraphDto();
            dto.Attended = attended;
            dto.Missed = Missed;
            return Ok(dto);

            

        }

        #endregion

    }
}
