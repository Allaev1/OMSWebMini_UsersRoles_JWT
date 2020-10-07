using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OMSWebMini.Services.Authorization
{
    /// <summary>
    /// Returns symetric security key
    /// </summary>
    public interface ISigningSecurityKey
    {
        string SigningAlgorithm { get; }

        SecurityKey GetKey();
    }
}
