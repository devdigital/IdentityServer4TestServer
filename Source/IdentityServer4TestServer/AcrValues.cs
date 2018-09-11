{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// ACR values.
    /// </summary>
    internal class AcrValues
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AcrValues"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public AcrValues(string value)
        {
            this.Values = string.IsNullOrWhiteSpace(value)
                ? new Dictionary<string, string>()
                : value.Split(' ').ToDictionary(p => p.Split(':')[0], p => p.Split(':')[1]);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AcrValues"/> class.
        /// </summary>
        public AcrValues()
        {
            this.Values = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public IDictionary<string, string> Values { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The value.</returns>
        public TData GetValue<TData>(string key, TData defaultValue)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (!this.Values.ContainsKey(key))
            {
                return defaultValue;
            }

            try
            {
                var value = this.Values[key];
                return (TData)Convert.ChangeType(value, typeof(TData));
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Adds the specified key.
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

            if (this.Values.ContainsKey(key))
            {
                throw new InvalidOperationException(
                    string.Format("Key '{0}' already added as ACR value.", key));
            }

            this.Values.Add(key, value);
            return this;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return string.Join(" ", this.Values.Select(kvp => string.Format("{0}:{1}", kvp.Key, kvp.Value)));
        }
    }
}