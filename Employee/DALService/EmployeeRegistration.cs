using Dapper;
using Employee.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Employee.DALService
{
    public class EmployeeRegistration
    {
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=EmployeeDetailDatabase;Integrated Security=True");
        public ResponseData SaveEmployee(EmployeeModel employee)
        {
            using (var tran = con.BeginTransaction())
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
                    ResponseData result = SqlMapper.Query<ResponseData>(con, "[dbo].[usp_iud_employee_info]", param, tran,
                      commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (result.IsSuccess && employee.Action != "D")
                    {
                        foreach (AcademicInformationModel academicInfo in employee.AcademicInformations)
                        {
                            academicInfo.EmployeeId = employee.Action == "A" ? result.Id : employee.EmployeeId;
                            SaveAcademicInformations(academicInfo, tran);
                        }
                    }
                    tran.Commit();
                    return result;
                }

                catch (Exception ex)
                {
                    tran.Rollback();
                    throw;
                }
            }

        }
        private ResponseData SaveAcademicInformations(AcademicInformationModel academicInfo, SqlTransaction tran)
        {

            DynamicParameters param = new DynamicParameters();
            param.Add("@p_Action", academicInfo.Action);
            param.Add("@p_EmployeeId", academicInfo.EmployeeId);
            param.Add("@p_AId", academicInfo.AId);
            param.Add("@p_QId", academicInfo.Qualification.QId);
            param.Add("@p_MarkdObtained", academicInfo.MarksObtained);
            ResponseData result = SqlMapper.Query<ResponseData>(con, "[dbo].[usp_iud_academic_info]", param, tran,
             commandType: CommandType.StoredProcedure).FirstOrDefault();
            return result;

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
