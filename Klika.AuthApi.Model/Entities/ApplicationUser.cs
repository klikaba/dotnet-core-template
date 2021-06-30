using Microsoft.AspNetCore.Identity;

namespace Klika.AuthApi.Model.Entities
{
    public class ApplicationUser : IdentityUser 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
