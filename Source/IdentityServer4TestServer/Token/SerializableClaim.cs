// <copyright file="SerializableClaim.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer.Token
{
    using System.Security.Claims;

    /// <summary>
    /// Serializable claim.
    /// </summary>
    /// <seealso cref="System.Security.Claims.Claim" />
    public class SerializableClaim : Claim
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableClaim"/> class.
        /// </summary>
        /// <param name="type">The claim type.</param>
        /// <param name="value">The claim value.</param>
        /// <param name="valueType">The claim value type. If this parameter is null, then <see cref="F:System.Security.Claims.ClaimValueTypes.String"></see> is used.</param>
        /// <param name="issuer">The claim issuer. If this parameter is empty or null, then <see cref="F:System.Security.Claims.ClaimsIdentity.DefaultIssuer"></see> is used.</param>
        /// <param name="originalIssuer">The original issuer of the claim. If this parameter is empty or null, then the <see cref="P:System.Security.Claims.Claim.OriginalIssuer"></see> property is set to the value of the <see cref="P:System.Security.Claims.Claim.Issuer"></see> property.</param>
        public SerializableClaim(string type, string value, string valueType, string issuer, string originalIssuer)
            : base(type, value, valueType, issuer, originalIssuer)
        {
        }
    }
}