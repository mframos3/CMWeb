using System;
using System.Collections.Generic;
using CMWeb.Models;
using Microsoft.AspNetCore.Identity;

namespace CMWeb.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the CMWebUser class
    public class CMWebUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
        
        public ICollection<EventUser> EventUsers { get; set; }
        
    }
}