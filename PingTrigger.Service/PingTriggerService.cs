/**************************************************************************
 * Filename: PingTriggerService.cs
 * Project:  PingTrigger.exe
 * 
 * Description:
 * Main service class.  Runs on a periodic timer to kick the actions of 
 * the service. 
**************************************************************************/
using System;
using Serilog;
using Ninject;

namespace PingTrigger
{
    public class PingTriggerService
    {
        protected System.Timers.Timer _timer;
        private bool processLock;
        protected int sendBatchSize;

        /// <summary>
        /// Constructor for the service class. creates the service timer and ties it to the processing method
        /// </summary>
        public PingTriggerService()
        {
            Log.Information("Construction of PingTriggerService");
        }

        /// <summary>
        /// Start the service by kicking the timer
        /// </summary>
        public void Start()
        {
            Log.Warning("Starting service on {0}", System.Environment.MachineName);

            int dwellTime = 5000;
            Log.Information("Message service dwell timer {0} msec.", dwellTime);
            _timer = new System.Timers.Timer(dwellTime) { AutoReset = true };
            _timer.Elapsed += (sender, eventArgs) => ProcessMessages();
            _timer.Start();

        }

        /// <summary>
        /// Stop the service by killing the timer
        /// </summary>
        public void Stop()
        {
            Log.Warning("Stopping service on {0}", System.Environment.MachineName);
            _timer.Stop();
        }


        /// <summary>
        /// Process messages.  This process looks for messages to send and sends a block 
        /// of the configured batch size.  It is called based on the service timer and
        /// blocks re-entry
        /// </summary>
        public void ProcessMessages()
        {
            // primitively prevent re-entry into the processing
            if (processLock)
            {
                Log.Debug("Re-entry into service process, still busy.");
                return;
            }
            Log.Verbose("BEGIN: ProcessMessages");
            processLock = true;
            try
            {
            }
            catch (Exception e)
            {
                Log.Error("Unhandled exception in subtxtr service processing {0}", e);
            }
            processLock = false;
            Log.Verbose("END: ProcessMessages");
        }
    }
}
