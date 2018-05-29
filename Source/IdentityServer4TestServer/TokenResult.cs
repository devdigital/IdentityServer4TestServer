// <copyright file="TokenResult.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer
{
    using System;

    /// <summary>
    /// Token result.
    /// </summary>
    public class TokenResult
    {
        private TokenResult(bool isSuccess, string token, string errorMessage)
        {
            this.IsSuccess = isSuccess;
            this.Token = token;
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is success; otherwise, <c>false</c>.
        /// </value>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public string Token { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; }

        /// <summary>
        /// Success token result.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>The token result.</returns>
        public static TokenResult Success(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            return new TokenResult(isSuccess: true, token: token, errorMessage: null);
        }

        /// <summary>
        /// Failure token result.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>The token result.</returns>
        public static TokenResult Failure(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentNullException(nameof(errorMessage));
            }

            return new TokenResult(isSuccess: false, token: null, errorMessage: errorMessage);
        }
    }
}