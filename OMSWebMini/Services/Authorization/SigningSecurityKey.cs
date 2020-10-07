using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMSWebMini.Services.Authorization
{
    public class SigningSecurityKey : ISigningSecurityKey
    {
        SymmetricSecurityKey securityKey;
        string signingAlgorithm;

        public SigningSecurityKey(string symetricKey, string signingAlgorithm)
        {
            securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symetricKey));
            this.signingAlgorithm = signingAlgorithm;
        }

        public string SigningAlgorithm => signingAlgorithm;

        public SecurityKey GetKey()
        {
            return securityKey;
        }
    }
}
