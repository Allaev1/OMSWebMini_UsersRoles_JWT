using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OMSWebMini.Authentication.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string Login { set; get; }
    }
}
