
using System.ComponentModel.DataAnnotations;

namespace NexusERP_D2.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string EmployeeNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [StringLength(100)]
        public string Department { get; set; }

        [Required]
        [StringLength(100)]
        public string Designation { get; set; }

        [DataType(DataType.Date)]
        public DateTime JoiningDate { get; set; }

        [Required]
        public string EmploymentStatus { get; set; }

        // Optional link to an Identity account
        public string? ApplicationUserId { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }
    }
}
