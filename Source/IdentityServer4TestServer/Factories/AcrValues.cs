// <copyright file="AcrValues.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// ACR values.
    /// </summary>
    public class AcrValues
    {
        private readonly IDictionary<string, string> values;

        /// <summary>
        /// Initializes a new instance of the <see cref="AcrValues"/> class.
        /// </summary>
        public AcrValues()
        {
            this.values = new Dictionary<string, string>();
        }

        /// <summary>
        /// Adds the specified key/value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The ACR values.</returns>
        public AcrValues Add(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (this.values.ContainsKey(key))
            {
                throw new InvalidOperationException($"Key '{key}' already added as ACR value.");
            }

            this.values.Add(key, value);
            return this;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var pairs = this.values.Select(kvp => $"{kvp.Key}:{kvp.Value}");
            return string.Join(" ", pairs);
        }
    }
}