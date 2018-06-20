// <copyright file="ModelStateExtensions.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Humanizer;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    /// <summary>
    /// Model state extensions.
    /// </summary>
    internal static class ModelStateExtensions
    {
        /// <summary>
        /// Converts model state to validation error details.
        /// </summary>
        /// <param name="modelStateDictionary">The model state dictionary.</param>
        /// <param name="message">The message.</param>
        /// <param name="resource">The resource.</param>
        /// <returns>The validation error details.</returns>
        public static ValidationErrorDetails ToValidationErrorDetails(
            this ModelStateDictionary modelStateDictionary,
            string message,
            string resource)
        {
            if (modelStateDictionary == null)
            {
                throw new ArgumentNullException(nameof(modelStateDictionary));
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (string.IsNullOrWhiteSpace(resource))
            {
                throw new ArgumentNullException(nameof(resource));
            }

            var erroredModelStates = modelStateDictionary.Where(m => m.Value.Errors.Any()).ToList();

            var validationErrors = new List<ValidationError>();
            foreach (var erroredModelState in erroredModelStates)
            {
                foreach (var error in erroredModelState.Value.Errors)
                {
                    validationErrors.Add(new ValidationError(
                        resource,
                        ToField(erroredModelState.Key),
                        ToValidationErrorCode(error),
                        error.ErrorMessage));
                }
            }

            return new ValidationErrorDetails(message, validationErrors);
        }

        private static string ToField(string modelStateKey)
        {
            var lastPeriodIndex = modelStateKey.LastIndexOf(".", StringComparison.Ordinal);
            var fieldName = lastPeriodIndex == -1 ? modelStateKey : modelStateKey.Substring(lastPeriodIndex + 1);
            return fieldName.Camelize();
        }

        private static string ToValidationErrorCode(ModelError error)
        {
            if (error.Exception != null)
            {
                return "invalid";
            }

            if (string.IsNullOrWhiteSpace(error.ErrorMessage))
            {
                return "invalid";
            }

            if (error.ErrorMessage.Contains("required"))
            {
                return "missing-field";
            }

            return "invalid";
        }
    }
}