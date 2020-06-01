using System;
using System.Diagnostics;
using Serilog;

namespace Banking.Customers.Middleware
{
    public class PerformanceTracker
    {
        private readonly string _trackedAction;
        private readonly Stopwatch _tracker;
        private readonly string _infoName;
        private readonly string _infoValue;

        /// <inheritdoc />
        public PerformanceTracker(string trackedAction) : this(trackedAction, null, null)
        {
        }

        /// <summary>
        /// Creates a new object to track performance.  The constructor starts the clock ticking.
        /// </summary>
        /// <param name="trackedAction">The name of the action you're tracking performance for like API method name, procname, or other.</param>
        /// <param name="infoName">The name of an additional value you want to capture</param>
        /// <param name="infoValue">The value of the additional info you're capturing (like parameters for a method)</param>
        public PerformanceTracker(string trackedAction, string infoName, string infoValue)
        {
            _infoName = infoName;
            _infoValue = infoValue;
            if (string.IsNullOrEmpty(infoValue) && !string.IsNullOrEmpty(infoName) ||
                !string.IsNullOrEmpty(infoValue) && string.IsNullOrEmpty(infoName))
            {
                throw new ArgumentException("Either both infoName and infoValue must be provided or neither should be provided.");
            }
            _trackedAction = trackedAction;
            _tracker = new Stopwatch();
            _tracker.Start();
        }

        public void Stop()
        {
            if (_tracker == null) return;

            _tracker.Stop();
            if (string.IsNullOrEmpty(_infoValue))
            {
                Log.Information($"Tracking Process\"|AppOperation=\"{_trackedAction}\"|ProcessTime=\"{_tracker.ElapsedMilliseconds}");
            }
            else
            {
                Log.Information($"Tracking Process with {_infoName} of {_infoValue}\"|AppOperation=\"{_trackedAction}\"|ProcessTime=\"{_tracker.ElapsedMilliseconds}");
            }
        }
    }

}
