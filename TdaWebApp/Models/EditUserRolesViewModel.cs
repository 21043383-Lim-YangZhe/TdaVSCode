namespace TdaWebApp.Models
{
    public class EditUserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public List<string> AllRoles { get; set; }
        public List<string> SelectedRoles { get; set; }
    }
}
