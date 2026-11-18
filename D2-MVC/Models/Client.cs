using System.ComponentModel.DataAnnotations;

namespace NexusERP_D2.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Company / Client Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(30)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Address { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Client Type")]
        public string ClientType { get; set; } = "Business";

        [DataType(DataType.Date)]
        [Display(Name = "Registration Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Today;
    }
}