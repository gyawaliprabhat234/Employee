using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee.Models
{
    public class AcademicInformationModel
    {
        public string Action { get; set; }
        public int EmployeeId { get; set; }
        public int AId { get; set; }
        public int QId { get; set; }
      public  QualificationModel Qualification { get; set; }
        public int MarksObtained { get; set; }
    }
}
