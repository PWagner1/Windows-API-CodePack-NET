//Copyright (c) Microsoft Corporation.  All rights reserved.

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
#pragma warning disable CS8600, CS8605
namespace Microsoft.WindowsAPICodePack.ApplicationServices;

/// <summary>
/// This class generates .NET events based on Windows messages.  
/// The PowerRegWindow class processes the messages from Windows.
/// </summary>
internal static class MessageManager
{
    private static object _lockObject = new();
    private static PowerRegWindow? _window;

    #region Internal static methods

    /// <summary>
    /// Registers a callback for a power event.
    /// </summary>
    /// <param name="eventId">Guid for the event.</param>
    /// <param name="eventToRegister">Event handler for the specified event.</param>
    internal static void RegisterPowerEvent(Guid eventId, EventHandler eventToRegister)
    {
        EnsureInitialized();
        _window?.RegisterPowerEvent(eventId, eventToRegister);
    }

    /// <summary>
    /// Unregisters an event handler for a power event.
    /// </summary>
    /// <param name="eventId">Guid for the event.</param>
    /// <param name="eventToUnregister">Event handler to unregister.</param>
    internal static void UnregisterPowerEvent(Guid eventId, EventHandler eventToUnregister)
    {
        EnsureInitialized();
        _window?.UnregisterPowerEvent(eventId, eventToUnregister);
    }

    #endregion

    /// <summary>
    /// Ensures that the hidden window is initialized and 
    /// listening for messages.
    /// </summary>
    private static void EnsureInitialized()
    {
        lock (_lockObject)
        {
            if (_window == null)
            {
                // Create a new hidden window to listen
                // for power management related window messages.
                _window = new();
            }
        }
    }

    /// <summary>
    /// Catch Windows messages and generates events for power specific
    /// messages.
    /// </summary>
    internal class PowerRegWindow : Form
    {
        private Hashtable _eventList = new();
        private ReaderWriterLock _readerWriterLock = new();

        internal PowerRegWindow()
            : base()
        {

        }

        #region Internal Methods

        /// <summary>
        /// Adds an event handler to call when Windows sends 
        /// a message for an event.
        /// </summary>
        /// <param name="eventId">Guid for the event.</param>
        /// <param name="eventToRegister">Event handler for the event.</param>
        // ReSharper disable once MemberHidesStaticFromOuterClass
        internal void RegisterPowerEvent(Guid eventId, EventHandler eventToRegister)
        {
            _readerWriterLock.AcquireWriterLock(Timeout.Infinite);
            if (!_eventList.Contains(eventId))
            {
                Power.RegisterPowerSettingNotification(Handle, eventId);
                ArrayList newList = new();
                newList.Add(eventToRegister);
                _eventList.Add(eventId, newList);
            }
            else
            {
                // ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (_eventList != null)
                    // ReSharper restore ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                {
                    ArrayList? currList = (ArrayList)_eventList[eventId];
                    currList?.Add(eventToRegister);
                }
            }
            _readerWriterLock.ReleaseWriterLock();
        }

        /// <summary>
        /// Removes an event handler.
        /// </summary>
        /// <param name="eventId">Guid for the event.</param>
        /// <param name="eventToUnregister">Event handler to remove.</param>
        /// <exception cref="InvalidOperationException">Cannot unregister 
        /// a function that is not registered.</exception>
        // ReSharper disable MemberHidesStaticFromOuterClass
        internal void UnregisterPowerEvent(Guid eventId, EventHandler eventToUnregister)
            // ReSharper restore MemberHidesStaticFromOuterClass
        {
            _readerWriterLock.AcquireWriterLock(Timeout.Infinite);
            if (_eventList.Contains(eventId))
            {
                ArrayList currList = (ArrayList)_eventList[eventId];
                currList?.Remove(eventToUnregister);
            }
            else
            {
                throw new InvalidOperationException(LocalizedMessages.MessageManagerHandlerNotRegistered);
            }
            _readerWriterLock.ReleaseWriterLock();
        }

        #endregion

        /// <summary>
        /// Executes any registered event handlers.
        /// </summary>
        /// <param name="eventHandlerList">ArrayList of event handlers.</param>            
        private static void ExecuteEvents(ArrayList? eventHandlerList)
        {
            if (eventHandlerList != null)
            {
                foreach (EventHandler handler in eventHandlerList)
                {
                    handler?.Invoke(null, new());
                }
            }
        }

        /// <summary>
        /// This method is called when a Windows message 
        /// is sent to this window.
        /// The method calls the registered event handlers.
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            // Make sure it is a Power Management message.
            if (m.Msg == PowerManagementNativeMethods.PowerBroadcastMessage &&
                (int)m.WParam == PowerManagementNativeMethods.PowerSettingChangeMessage)
            {
                PowerManagementNativeMethods.PowerBroadcastSetting ps =
                    (PowerManagementNativeMethods.PowerBroadcastSetting)Marshal.PtrToStructure(
                        m.LParam, typeof(PowerManagementNativeMethods.PowerBroadcastSetting));

                IntPtr pData = new(m.LParam.ToInt64() + Marshal.SizeOf(ps));
                Guid currentEvent = ps.PowerSetting;

                // IsMonitorOn
                if (ps.PowerSetting == EventManager.MonitorPowerStatus &&
                    ps.DataLength == Marshal.SizeOf(typeof(int)))
                {
                    int monitorStatus = (int)Marshal.PtrToStructure(pData, typeof(int));
                    PowerManager.IsMonitorOn = monitorStatus != 0;
                    EventManager.MonitorOnReset.Set();
                }

                if (!EventManager.IsMessageCaught(currentEvent))
                {
                    ExecuteEvents(_eventList[currentEvent] as ArrayList);
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

    }
}