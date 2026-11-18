
namespace NexusERP_D2.Models.ViewModels
{
    public class UserRowViewModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Department { get; set; }

        public string Role { get; set; }
    }

    public class UserManagementViewModel
    {
        public List<UserRowViewModel> Users { get; set; }
            = new List<UserRowViewModel>();
    }
}
