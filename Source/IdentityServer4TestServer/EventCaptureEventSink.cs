// <copyright file="EventCaptureEventSink.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace IdentityServer4TestServer
{
    using System;
    using System.Threading.Tasks;
    using IdentityServer4.Events;
    using IdentityServer4.Services;

    /// <summary>
    /// Event capture event sink.
    /// Stores any Identity Server events.
    /// </summary>
    /// <seealso cref="IdentityServer4.Services.IEventSink" />
    public class EventCaptureEventSink : IEventSink
    {
        /// <summary>
        /// The event capture.
        /// </summary>
        private readonly IdentityServerEventCapture eventCapture;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventCaptureEventSink"/> class.
        /// </summary>
        /// <param name="eventCapture">The event capture.</param>
        public EventCaptureEventSink(IdentityServerEventCapture eventCapture)
        {
            this.eventCapture = eventCapture ?? throw new ArgumentNullException(nameof(eventCapture));
        }

        /// <inheritdoc />
        public Task PersistAsync(Event evt)
        {
            this.eventCapture.AddEvent(evt);
            return Task.CompletedTask;
        }
    }
}