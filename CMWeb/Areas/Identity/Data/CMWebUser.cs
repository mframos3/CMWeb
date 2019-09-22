using System;
using Microsoft.AspNetCore.Identity;

namespace CMWeb.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the CMWebUser class
    public class CMWebUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
        
    }
}