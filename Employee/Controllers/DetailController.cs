using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Employee.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Employee.DALService;

namespace Employee.Controllers
{
    public class DetailController : Controller
    {
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=EmployeeDetailDatabase;Integrated Security=True");

        EmployeeRegistration employeeRegistration = new EmployeeRegistration();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PracticePart()
        {
            return View();
        }
        public List<QualificationModel> GetAllQualifications()
        {
            try
            {
                List<QualificationModel> qList = SqlMapper.Query<QualificationModel>(con, "usp_get_all_qualifications",
                    commandType: CommandType.StoredProcedure).ToList();
                return qList;

            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public List<EmployeeModel> GetAllEmployeeDetail()
        {
            try
            {
                List<EmployeeModel> allList = SqlMapper.Query<EmployeeModel>(con, "dbo.usp_get_all_employees",
                    commandType: CommandType.StoredProcedure).ToList() ;
                return allList;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public ResponseData SaveEmployee(EmployeeModel employee)
        {
           
            return employeeRegistration.SaveEmployee(employee);
            
        }
     

        public EmployeeModel GetEmployeeById(int EmployeeId)
        {

            return employeeRegistration.GetEmployeeById(EmployeeId);
        }
        public List<AcademicInformationModel> GetEmployeeAcademicInformationById(int EmployeeId)
        {
            return employeeRegistration.GetEmployeeAcademicInformationById(EmployeeId);
        }
       


    }
}
