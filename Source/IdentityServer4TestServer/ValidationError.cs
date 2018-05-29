// <copyright file="ValidationError.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer
{
    using System;

    /// <summary>
    /// Validation error.
    /// </summary>
    internal class ValidationError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationError"/> class.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="field">The field.</param>
        /// <param name="code">The code.</param>
        /// <param name="errorMessage">The error message.</param>
        public ValidationError(string resource, string field, string code, string errorMessage = null)
        {
            if (string.IsNullOrWhiteSpace(resource))
            {
                throw new ArgumentNullException(nameof(resource));
            }

            if (string.IsNullOrWhiteSpace(field))
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            this.Resource = resource;
            this.Field = field;
            this.Code = code;
            this.Message = errorMessage;
        }

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <value>
        /// The resource.
        /// </value>
        public string Resource { get; }

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <value>
        /// The field.
        /// </value>
        public string Field { get; }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; }
    }
}