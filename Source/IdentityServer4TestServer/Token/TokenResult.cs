// <copyright file="TokenResult.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer.Token
{
    using System;
    using System.Net;

    /// <summary>
    /// Token result.
    /// </summary>
    public class TokenResult
    {
        private TokenResult(bool isSuccess, string token, HttpStatusCode statusCode, string responseBody)
        {
            this.IsSuccess = isSuccess;
            this.Token = token;
            this.StatusCode = statusCode;
            this.ResponseBody = responseBody;
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
        /// Gets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ResponseBody { get; }

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

            return new TokenResult(
                isSuccess: true,
                token: token,
                statusCode: HttpStatusCode.OK,
                responseBody: null);
        }

        /// <summary>
        /// Failure token result.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="responseBody">The response body.</param>
        /// <returns>
        /// The token result.
        /// </returns>
        public static TokenResult Failure(HttpStatusCode statusCode, string responseBody)
        {
            return new TokenResult(
                isSuccess: false,
                token: null,
                statusCode: statusCode,
                responseBody: responseBody);
        }
    }
}