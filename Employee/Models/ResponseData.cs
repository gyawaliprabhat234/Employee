using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employee.Models
{
    public class ResponseData
    {
        public string  Message { get; set; }
        public bool IsSuccess { get; set; }
        public object Records { get; set; }
        public int Id { get; set; }
    }
}
