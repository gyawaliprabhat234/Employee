using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Employee.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
namespace Employee.Controllers
{
    public class DetailController : Controller
    {
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=EmployeeDetailDatabase;Integrated Security=True");
      
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
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@p_Action", employee.Action);
                param.Add("@p_EmployeeId", employee.EmployeeId);
                param.Add("@p_Name", employee.Name);
                param.Add("@p_DateOfBirth", employee.DateOfBirth);
                param.Add("@p_EmailAddress", employee.EmailAddress);
                param.Add("@p_Salary", employee.Salary);
                ResponseData result = SqlMapper.Query<ResponseData>(con, "[dbo].[usp_iud_employee_info]",param,
                  commandType: CommandType.StoredProcedure).FirstOrDefault();
                if (result.IsSuccess)
                {
                    foreach(AcademicInformationModel academicInfo in employee.AcademicInformations)
                    {
                        academicInfo.EmployeeId = employee.Action == "A" ?  result.Id : employee.EmployeeId;
                        SaveAcademicInformations(academicInfo);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
       public ResponseData SaveAcademicInformations(AcademicInformationModel academicInfo)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@p_Action",academicInfo.Action);
                param.Add("@p_EmployeeId",academicInfo.EmployeeId);
                param.Add("@p_AId",academicInfo.AId);
                param.Add("@p_QId",academicInfo.Qualification.QId);
                param.Add("@p_MarkdObtained",academicInfo.MarksObtained);
                ResponseData result = SqlMapper.Query<ResponseData>(con, "[dbo].[usp_iud_academic_info]",param,
                 commandType: CommandType.StoredProcedure).FirstOrDefault();
                return result; 
            }
            catch (Exception)
            {

                throw;
            }
        }

        public EmployeeModel GetEmployeeById(int EmployeeId)
        {
            if (EmployeeId <= 0)
                return null;
            DynamicParameters param = new DynamicParameters();
            param.Add("@p_EmployeeId", EmployeeId);
            EmployeeModel employee = SqlMapper.Query<EmployeeModel>(con, "usp_get_employee_by_id", param,
                commandType: CommandType.StoredProcedure).FirstOrDefault();
            employee.AcademicInformations = GetEmployeeAcademicInformationById(EmployeeId);
            return employee;

            
        }
        public List<AcademicInformationModel> GetEmployeeAcademicInformationById(int EmployeeId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@p_EmployeeId", EmployeeId);
            return SqlMapper.Query<AcademicInformationModel>(con, "usp_get_employee_academic_info_by_id", param,
                commandType: CommandType.StoredProcedure).ToList();
        }

        
    }
}
