﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Learn2CodeAPI.Data;
using Learn2CodeAPI.Data.Mapper;
using Learn2CodeAPI.Dtos.AdminDto;
using Learn2CodeAPI.IRepository.Generic;
using Learn2CodeAPI.IRepository.IRepositoryAdmin;
using Learn2CodeAPI.IRepository.IRepositoryStudent;
using Learn2CodeAPI.Models.Admin;
using Learn2CodeAPI.Models.Login.Identity;
using Learn2CodeAPI.Models.Student;
using Learn2CodeAPI.Models.Tutor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Learn2CodeAPI.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private  IMapper mapper;
        private IGenRepository<University> universityGenRepo;
        private IGenRepository<Degree> DegreeGenRepo;
        private IGenRepository<Module> ModuleGenRepo;
        private IGenRepository<Student> StudentGenRepo;
        private IGenRepository<CourseFolder> CourseFolderGenRepo;
        private IGenRepository<CourseSubCategory> CourseSubCategoryGenRepo;
        private IGenRepository<SessionContentCategory> SessionContentCategoryRepo;
        private IGenRepository<Tutor> TutorGenRepo;
        private readonly AppDbContext db;
        private IAdmin AdminRepo;
        public AdminController(
            UserManager<AppUser> userManager,
            IMapper _mapper,
            IGenRepository<University> _universityGenRepo,
            IGenRepository<Degree> _DegreeGenRepo,
            IGenRepository<Module> _ModuleGenRepo,
            IGenRepository<CourseFolder> _CourseFolderGenRepo,
             IGenRepository<CourseSubCategory> _CourseSubCategoryGenRepo,
            IGenRepository<Student> _StudentGenRepo,
             IGenRepository<Tutor> _TutorGenRepo,
            IGenRepository<SessionContentCategory> _SessionContentCategoryRepo,
            IAdmin _AdminRepo,
            AppDbContext _db

            )
        

        
        {
            _userManager = userManager; 
            db = _db;
            CourseSubCategoryGenRepo = _CourseSubCategoryGenRepo;
            universityGenRepo = _universityGenRepo;
            mapper = _mapper;
            DegreeGenRepo = _DegreeGenRepo;
            ModuleGenRepo = _ModuleGenRepo;
            AdminRepo = _AdminRepo;
            CourseFolderGenRepo = _CourseFolderGenRepo;
            StudentGenRepo = _StudentGenRepo;
            TutorGenRepo = _TutorGenRepo;
            SessionContentCategoryRepo = _SessionContentCategoryRepo;
        }

        #region University

        [HttpGet]
        [Route("GetUniversitybyId/{UniversityId}")]
        public async Task<IActionResult>GetUniversitybyId(int UniversityId)
        {
            var entity  = await universityGenRepo.Get(UniversityId);

            return Ok(entity);
        }

        [HttpGet]
        [Route("SearchUniversity/{UniversityName}")]
        public async Task<IActionResult> SearchUniversity(string UniversityName)
        {
            var entity = await AdminRepo.GetByName(UniversityName);

            return Ok(entity);
        }

        [HttpGet]
        [Route("GetAllUniversities")]
        public async Task<IActionResult> GetAllUniversities()
        {
            var Universities = await universityGenRepo.GetAll();
            return Ok(Universities);

        }

        [HttpPost]
        [Route("CreateUniversity")]
        public async Task<IActionResult> CreateUniversity([FromBody] UniversityDto dto)
        {
            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                var check = db.University.Where(zz => zz.UniversityName == dto.UniversityName).FirstOrDefault();
                if (check != null)
                {
                    result.message = "University already exists";
                    return BadRequest(result.message);
                }
                University entity = mapper.Map<University>(dto);
                var data = await universityGenRepo.Add(entity);
                result.data = data;
                result.message = "university created";
                return Ok(result);
            }
            catch 
            {

                result.message = "Something went wrong creating the university";
                return BadRequest(result.message);
            }
          
        }

        [HttpPut]
        [Route("EditUniversity")]
        public async Task<IActionResult> EditUniversity([FromBody] UniversityDto dto)
        {
            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var check = db.University.Where(zz => zz.UniversityName == dto.UniversityName).FirstOrDefault();
                if (check != null)
                {
                    result.message = "University already exists";
                    return BadRequest(result.message);
                }
                University entity = mapper.Map<University>(dto);
                var data = await universityGenRepo.Update(entity);
                result.data = data;
                result.message = "university updated";
                return Ok(result);

            }
            catch  
            {
                result.message = "Something went wrong updating the university";
                return BadRequest(result.message);

            }
           


            
        }

       

        [HttpDelete]
        [Route("DeleteUniversity/{UniversityId}")]
        public async Task<IActionResult> DeleteUniversity(int UniversityId)
        {
            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var data = await universityGenRepo.Delete(UniversityId);
                result.data = data;
                result.message = "University deleted";
                return Ok(result);
            }
            catch 
            {

                result.message = "Something went wrong deleting the university";
                return BadRequest(result.message);
            }

           
        }

        #endregion

        #region Degree

        [HttpGet]
        [Route("GetDegreebyId/{DegreeId}")]
        public async Task<IActionResult> GetDegreebyId(int DegreeId)
        {
            var entity = await DegreeGenRepo.Get(DegreeId);
           

            return Ok(entity);
        }

        [HttpGet]
        [Route("SearchDegree/{DegreeName}")]
        public async Task<IActionResult> SearchDegree(string DegreeName)
        {
            var entity = await AdminRepo.GetByDegreeName(DegreeName);

            return Ok(entity);
        }


        [HttpGet]
        [Route("GetAllDegrees/{UniversityId}")]
        public async Task<IActionResult> GetAllDegrees(int UniversityId)
        {

            var degrees = await AdminRepo.GetAllDegrees(UniversityId);
            return Ok(degrees);

        }

        [HttpPost]
        [Route("CreateDegree")]
        public async Task<IActionResult> CreateDegree([FromBody] DegreeDto dto)
        {
            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                var check = db.Degrees.Where(zz => zz.DegreeName == dto.DegreeName).FirstOrDefault();
                if (check != null)
                {
                    result.message = "Degree already exists";
                    return BadRequest(result.message);
                }
                Degree entity = mapper.Map<Degree>(dto);
                var data = await DegreeGenRepo.Add(entity);
                result.data = data;
                result.message = "Degree created";
                return Ok(result);
            }
            catch
            {

                result.message = "something went wrong creating the degree";
                return BadRequest(result.message);
            }

        }


        [HttpPut]
        [Route("EditDegree")]
        public async Task<IActionResult> EditDegree([FromBody] DegreeDto dto)
        {
            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var check = db.Degrees.Where(zz => zz.DegreeName == dto.DegreeName).FirstOrDefault();
                if (check != null)
                {
                    result.message = "Degree already exists";
                    return BadRequest(result.message);
                }
                Degree entity = mapper.Map<Degree>(dto);
                var data = await DegreeGenRepo.Update(entity);
                result.data = data;
                result.message = "Degree updated";
                return Ok(result);

            }
            catch
            {
                result.message = "something went wrong updating the degree";
                return BadRequest(result.message);

            }

        }

      
        

        [HttpDelete]
        [Route("DeleteDegree/{DegreeId}")]
        public async Task<IActionResult> DeleteDegree(int DegreeId)
        {
            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var data = await DegreeGenRepo.Delete(DegreeId);
                result.data = data;
                result.message = "Degree deleted";
                return Ok(result);
            }
            catch
            {

                result.message = "something went wrong deleting the degree";
                return BadRequest(result.message);
            }
           
        }

        #endregion

        #region Module

        [HttpGet]
        [Route("SearchModule/{ModuleName}")]
        public async Task<IActionResult> SearchModule(string ModuleName)
        {
            var entity = await AdminRepo.GetByModuleName(ModuleName);

            return Ok(entity);
        }

        [HttpGet]
        [Route("GetModulebyId/{ModuleId}")]
        public async Task<IActionResult> GetModulebyId(int ModuleId)
        {
            var entity = await ModuleGenRepo.Get(ModuleId);
           

            return Ok(entity);
        }



        [HttpGet]
        [Route("GetAllModules/{DegreeId}")]
        public async Task<IActionResult> GetAllModules(int DegreeId)
        {
            var modules = await AdminRepo.GetAllModules(DegreeId);
            return Ok(modules);

        }

        [HttpPost]
        [Route("CreateModule")]
        public async Task<IActionResult> CreateModule([FromBody] Module dto)
        {
            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                var check = db.Modules.Where(zz => zz.ModuleCode == dto.ModuleCode).FirstOrDefault();
                if (check != null)
                {
                    result.message = "Module already exists";
                    return BadRequest(result.message);
                }
                Module entity = mapper.Map<Module>(dto);
                var data = await ModuleGenRepo.Add(entity);
                result.data = data;
                result.message = "Module created";
                return Ok(result);
            }
            catch
            {

                result.message = "something went wrong creating the module";
                return BadRequest(result.message);
            }


        }

        [HttpPut]
        [Route("EditModule")]
        public async Task<IActionResult> EditModule([FromBody] Module dto)
        {
            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var check = db.Modules.Where(zz => zz.ModuleCode == dto.ModuleCode).FirstOrDefault();
                if (check != null)
                {
                    result.message = "Module already exists";
                    return BadRequest(result.message);
                }
                Module entity = mapper.Map<Module>(dto);
                var data = await ModuleGenRepo.Update(entity);
                result.data = data;
                result.message = "Module updated";
                return Ok(result);

            }
            catch
            {
                result.message = "something went wrong updating the module";
                return BadRequest(result.message);

            }
        }

       

        [HttpDelete]
        [Route("DeleteModule/{ModuleId}")]
        public async Task<IActionResult> DeleteModule(int ModuleId)
        {
            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var data = await ModuleGenRepo.Delete(ModuleId);
                result.data = data;
                result.message = "Module deleted";
                return Ok(result);
            }
            catch
            {

                result.message = "something went wrong deleting the module";
                return BadRequest(result.message);
            }
          
        }

        #endregion

        #region CourseFolder

        [HttpPost]
        [Route("CreateCourseFolder")]
        public async Task<IActionResult> CreateCourseFolder([FromBody] CourseFolderDto dto)
        {
            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                var check = db.courseFolders.Where(zz => zz.CourseFolderName == dto.CourseFolderName).FirstOrDefault();
                if (check != null)
                {
                    result.message = "Course folder already exists";
                    return BadRequest(result.message);
                }
                CourseFolder entity = mapper.Map<CourseFolder>(dto);
                var data = await CourseFolderGenRepo.Add(entity);
                result.data = data;
                result.message = "Course Folder created";
                return Ok(result);
            }
            catch
            {

                result.message = "something went wrong creating the course folder";
                return BadRequest(result.message);
            } 

        }

        [HttpGet]
        [Route("GetAllCourseFolder")]
        public async Task<IActionResult> GetAllCourseFolder()
        {
            var entity = await CourseFolderGenRepo.GetAll();
            return Ok(entity);

        }

        [HttpGet]
        [Route("SearchCourseFolder/{CourseFolderName}")]
        public async Task<IActionResult> SearchCourseFolder(string CourseFolderName)
        {
            var entity = await AdminRepo.GetByCourseFolderName(CourseFolderName);

            return Ok(entity);
        }

        [HttpPut]
        [Route("EditCourseFolder")]
        public async Task<IActionResult> EditCourseFolder([FromBody] CourseFolderDto dto)
        {
            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                var check = db.courseFolders.Where(zz => zz.CourseFolderName == dto.CourseFolderName).FirstOrDefault();
                if (check != null)
                {
                    result.message = "Course folder already exists";
                    return BadRequest(result.message);
                }
                CourseFolder entity = mapper.Map<CourseFolder>(dto);
                var data = await CourseFolderGenRepo.Update(entity);
                result.data = data;
                result.message = "Course Folder updated";
                return Ok(result);
            }
            catch
            {

                result.message = "something went wrong updating the course folder";
                return BadRequest(result.message);
            }

        }

        [HttpDelete]
        [Route("DeleteCourseFolder/{CourseFolderId}")]
        public async Task<IActionResult> DeleteCourseFolder(int CourseFolderId)
        {
            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var data = await CourseFolderGenRepo.Delete(CourseFolderId);
                result.data = data;
                result.message = "Module deleted";
                return Ok(result);
            }
            catch
            {

                result.message = "something went wrong deleting the module";
                return BadRequest(result.message);
            }
            
          
        }


        #endregion

        #region Student
        [HttpGet]
        [Route("GeAllStudents")]
        public async Task<IActionResult> GeAllStudents()
        {
            var entity = await AdminRepo.GetAllStudents();

            return Ok(entity);
        }


        
        //userid is in the aspnet users table
        [HttpDelete]
        [Route("DeleteStudent/{userId}")]
        public IActionResult search(string userId) {

            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var data = db.Users.Where(zz => zz.Id == userId).FirstOrDefault();
                db.Remove(data);
                db.SaveChanges();
                result.data = data;
                result.message = "Student deleted";
                return Ok(result);
            }
            catch
            {

                result.message = "Something went wrong deleting the student";
                return BadRequest(result.message);
            }
           
            
        }


        #endregion

        #region SessionContentCategory
        [HttpGet]
        [Route("GetAllSessionContentCategory")]
        public async Task<IActionResult> GetAllSessionContentCategory()
        {
            var Universities = await SessionContentCategoryRepo.GetAll();
            return Ok(Universities);

        }

        [HttpPost]
        [Route("CreateSessionContentCategory")]
        public async Task<IActionResult> CreateSessionContentCategory([FromBody] SessionContentCategoryDto dto)
        {
            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                var check = db.SessionContentCategory.Where(zz => zz.SessionContentCategoryName == dto.SessionContentCategoryName).FirstOrDefault();
                if (check != null)
                {
                    result.message = "Category already exists";
                    return BadRequest(result.message);
                }
                SessionContentCategory entity = mapper.Map<SessionContentCategory>(dto);
                var data = await SessionContentCategoryRepo.Add(entity);
                result.data = data;
                result.message = "Category created";
                return Ok(result);
            }
            catch
            {

                result.message = "something went wrong creating the category";
                return BadRequest(result.message);
            }
           

        }

        [HttpPut]
        [Route("EditSessionContentCategory")]
        public async Task<IActionResult> EditSessionContentCategory([FromBody] SessionContentCategoryDto dto)
        {
            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                var check = db.SessionContentCategory.Where(zz => zz.SessionContentCategoryName == dto.SessionContentCategoryName).FirstOrDefault();
                if (check != null)
                {
                    result.message = "Category already exists";
                    return BadRequest(result.message);
                }
                SessionContentCategory entity = mapper.Map<SessionContentCategory>(dto);
                var data = await SessionContentCategoryRepo.Update(entity);
                result.data = data;
                result.message = "Category updated";
                return Ok(result);
            }
            catch
            {

                result.message = "something went wrong updating the category";
                return BadRequest(result.message);
            }
            
        }

        [HttpDelete]
        [Route("DeleteSessionContentCategory/{SessionContentCategoryId}")]
        public async Task<IActionResult> DeleteSessionContentCategory(int SessionContentCategoryId)
        {
            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var data = await SessionContentCategoryRepo.Delete(SessionContentCategoryId);
                result.data = data;
                result.message = "Category deleted";
                return Ok(result);
            }
            catch
            {

                result.message = "something went wrong deleting the category";
                return BadRequest(result.message);
            }
           
        }
        #endregion

        #region CourseContentCategory
        [HttpGet]
        [Route("GetAllCourseSubCategory")]
        public async Task<IActionResult> GetAllCourseSubCategory()
        {
            var subcategories = await CourseSubCategoryGenRepo.GetAll();
            return Ok(subcategories);

        }

        
        [HttpPost]
        [Route("CreateCourseSubCategory")]
        public async Task<IActionResult> CreateCourseSubCategory([FromBody] CoursSubCategoryDto dto)
        {
            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var check = db.courseSubCategory.Where(zz => zz.CourseSubCategoryName == dto.CourseSubCategoryName).FirstOrDefault();
                if (check != null)
                {
                    result.message = "Subcategory already exists";
                    return BadRequest(result.message);
                }

                CourseSubCategory entity = mapper.Map<CourseSubCategory>(dto);
                var data = await CourseSubCategoryGenRepo.Add(entity);
                result.data = data;
                result.message = "course subcategory created";
                return Ok(result);
            }
            catch
            {

                result.message = "something went wrong creating the subcategory";
                return BadRequest(result.message);
            }

        }

        [HttpPut]
        [Route("EditCourseSubCategory")]
        public async Task<IActionResult> EditCourseSubCategory([FromBody] CoursSubCategoryDto dto)
        {
            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                CourseSubCategory entity = mapper.Map<CourseSubCategory>(dto);
                var data = await CourseSubCategoryGenRepo.Update(entity);
                result.data = data;
                result.message = "course subcategory updated";
                return Ok(result);
            }
            catch
            {

                result.message = "something went wrong updating the subcategory";
                return BadRequest(result.message);
            }

        }

        [HttpDelete]
        [Route("DeleteCourseSubCategory/{CourseSubCategoryId}")]
        public async Task<IActionResult> DeleteCourseSubCategory(int CourseSubCategoryId)
        {
            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var data = await CourseSubCategoryGenRepo.Delete(CourseSubCategoryId);
                result.data = data;
                result.message = "Subcategory deleted";
                return Ok(result);
            }
            catch
            {

                result.message = "something went wrong deleting the subcategory";
                return BadRequest(result.message);
            }

        }

        #endregion

        #region Tutor
        [HttpGet]
        [Route("GetAllAplications")]
        public async Task<IActionResult> GetAllAplications()
        {
            var applications = await AdminRepo.GetAllApplications();
            return Ok(applications);

        }

        [HttpGet]
        [Route("GetTutorbyId/{TutorId}")]
        public async Task<IActionResult> GetTutorbyId(int TutorId)
        {
            var entity = await TutorGenRepo.Get(TutorId);

            return Ok(entity);
        }

        //email will be done at later stage
        [HttpPut]
        [Route("RejectTutor")]
        public async Task<IActionResult> RejectTutor([FromBody] TutorDto dto)
        {
             dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Tutor entity = mapper.Map<Tutor>(dto);
                var data = await AdminRepo.Reject(entity);
                result.data = data;
                result.message = "Tutor rejected";
                return Ok(result);
            }
            catch
            {

                result.message = "something went wrong while rejectin the tutor";
                return BadRequest(result.message);
            }
          
        }

        [HttpPut]
        [Route("CreateTutor")]
        public async Task<IActionResult> CreateTutor([FromBody] CreateTutorDto dto)
        {

            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var check = db.Users.Where(zz => zz.UserName == dto.UserName).FirstOrDefault();
                if (check != null)
                {
                    result.message = "Username is taken";
                    return BadRequest(result.message);
                }

                var userIdentity = mapper.Map<AppUser>(dto);
                userIdentity.Email = dto.TutorEmail;
                var data = await AdminRepo.CreateTutor(userIdentity, dto);
                result.data = data;
                result.message = "Tutor created";
                return Ok(result);
            }
            catch
            {

                result.message = "something went wrong while creating the tutor";
                return BadRequest(result.message);
            }
           

        }


        [HttpGet]
        [Route("GetAllTutors")]
        public async Task<IActionResult> GetAllTutors()
        {
            var Tutors= await AdminRepo.GetAllTutors();
            return Ok(Tutors);

        }

        [HttpDelete]
        [Route("DeleteTutor/{userId}")]
        public IActionResult DeleteTutor(string userId)
        {
            dynamic result = new ExpandoObject();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = db.Users.Where(zz => zz.Id == userId).FirstOrDefault();
                var tutor = db.Tutor.Where(zz => zz.UserId == userId).FirstOrDefault();
                var x = db.File.Where(zz => zz.Id == tutor.FileId).FirstOrDefault();
                db.Remove(user);
                db.Remove(x);
                db.SaveChanges();
                result.data = tutor;
                result.message = "Tutor deleted";
                return Ok(result);
            }
            catch
            {

                result.message = "something went wrong while deleting the tutor";
                return BadRequest(result.message);
            }

        }


        #endregion

    }
}