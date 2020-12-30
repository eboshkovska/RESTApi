using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "10 character max")]
        public string Location { get; set; }

        public int Age { get; set; }
    }

    public class EmployeeDTO
    {

        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "10 character max")]
        public string Location { get; set; }
    }
}
