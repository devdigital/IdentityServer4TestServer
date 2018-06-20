// <copyright file="IdentityServerEventCapture.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using IdentityServer4.Events;

    /// <summary>
    /// Identity Server event capture.
    /// </summary>
    public class IdentityServerEventCapture
    {
        /// <summary>
        /// The events.
        /// </summary>
        private readonly List<Event> events;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityServerEventCapture"/> class.
        /// </summary>
        public IdentityServerEventCapture()
        {
            this.events = new List<Event>();
        }

        /// <summary>
        /// Adds an event.
        /// </summary>
        /// <param name="event">The event.</param>
        public void AddEvent(Event @event)
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            this.events.Add(@event);
        }

        /// <summary>
        /// Gets an event by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The event.</returns>
        public Event GetById(int id)
        {
            return this.events.FirstOrDefault(e => e.Id == id);
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <returns>The events.</returns>
        public IEnumerable<Event> GetEvents()
        {
            return this.events.AsReadOnly();
        }

        /// <summary>
        /// Determines whether the events contains event type.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <returns>
        ///   <c>true</c> if events contains event type; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsEventType(EventTypes eventType)
        {
            var @event = this.events.FirstOrDefault(e => e.EventType == eventType);
            return @event != null;
        }

        /// <summary>
        /// Determines whether the events contains message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns>
        ///   <c>true</c> if the events contains message; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsMessage(string message, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            var matchingEvents = this.events.Where(e => string.Compare(e.Message, message, comparison) == 0);
            return matchingEvents.Any();
        }

        /// <summary>
        /// Determines whether the events contains message starting with value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns>
        ///   <c>true</c> if events contains a message that starts with the specified value; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsMessageStartsWith(string value, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            var matchingEvents = this.events.Where(e => e.Message.StartsWith(value, comparison));
            return matchingEvents.Any();
        }
    }
}