using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee.Models
{
    public class EmployeeModel
    {
        public string Action { get; set; }
      public int EmployeeId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string EmailAddress { get; set; }
        public int Salary { get; set; }
        public List<AcademicInformationModel>  AcademicInformations{ get; set; }
    }
}
