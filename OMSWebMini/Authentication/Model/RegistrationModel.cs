using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OMSWebMini.Authentication.Model
{
    public class RegistrationModel
    {
        public string UserName { set; get; }

        public string Password { set; get; }

        public string Email { set; get; }

        public string Role { set; get; }
    }
}
