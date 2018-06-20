// <copyright file="UnprocessableEntityResult.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer.Validation
{
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Unprocessable entity result.
    /// </summary>
    /// <typeparam name="TResource">The type of the resource.</typeparam>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.IActionResult" />
    internal class UnprocessableEntityResult<TResource> : IActionResult
    {
        /// <inheritdoc />
        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = 422;

            var errors = context.ModelState.ToValidationErrorDetails(
                message: "There was a validation error.",
                resource: typeof(TResource).Name);

            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

            var content = JsonConvert.SerializeObject(errors, jsonSettings);

            response.ContentType = "application/json";
            response.ContentLength = Encoding.UTF8.GetByteCount(content);

            var data = Encoding.UTF8.GetBytes(content);
            await response.Body.WriteAsync(data, 0, data.Length);
        }
    }
}