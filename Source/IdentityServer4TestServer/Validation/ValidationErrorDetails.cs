// <copyright file="ValidationErrorDetails.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Validation error details.
    /// </summary>
    internal class ValidationErrorDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationErrorDetails"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ValidationErrorDetails(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            this.Message = message;
            this.Errors = Enumerable.Empty<ValidationError>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationErrorDetails"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="error">The error.</param>
        public ValidationErrorDetails(string message, ValidationError error)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            this.Message = message;
            this.Errors = new List<ValidationError> { error };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationErrorDetails"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errors">The errors.</param>
        public ValidationErrorDetails(string message, IEnumerable<ValidationError> errors)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            this.Message = message;
            this.Errors = errors ?? throw new ArgumentNullException(nameof(errors));
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public IEnumerable<ValidationError> Errors { get; }
    }
}