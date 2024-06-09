//Copyright (c) Microsoft Corporation.  All rights reserved.

using Microsoft.WindowsAPICodePack.ApplicationServices;
using Microsoft.WindowsAPICodePack.Shell;

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Microsoft.WindowsAPICodePack.Samples.PowerMgmtDemoApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 
    public partial class Window1 : Window
    {
        [DllImport("user32.dll")]
        private static extern int SendMessage(int hWnd, int hMsg, int wParam, int lParam);


        public delegate void MethodInvoker();

        private readonly MyPowerSettings _settings;
        private readonly BackgroundWorker _backgroundWorker = new BackgroundWorker();
        private string _cancelReason = string.Empty;
        private readonly DispatcherTimer _timerClock;

        public Window1()
        {
            InitializeComponent();
            _settings = (MyPowerSettings)FindResource("powerSettings");

            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.DoWork += backgroundWorker_DoWork;
            _backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

            // Start a timer/clock so we can periodically ping for the power settings.
            _timerClock = new DispatcherTimer();
            _timerClock.Interval = new TimeSpan(0, 0, 5);
            _timerClock.IsEnabled = true;
            _timerClock.Tick += TimerClock_Tick;

        }

        void TimerClock_Tick(object sender, EventArgs e)
        {
            GetPowerSettings();
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Once the thread is finished / i.e. indexing is done,
            // update our labels
            if (string.IsNullOrEmpty(_cancelReason))
            {
                SetLabelButtonStatus(IndexerCurrentFileLabel, "Indexing completed!");
                SetLabelButtonStatus(IndexerStatusLabel, "Click \"Start Search Indexer\" to run the indexer again.");
                SetLabelButtonStatus(StartStopIndexerButton, "Start Search Indexer!");
            }

            // Clear our the cancel reason as the operation has completed.
            _cancelReason = "";
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            SetLabelButtonStatus(IndexerCurrentFileLabel, "Running search indexer ....");

            IKnownFolder docs;

            if (ShellLibrary.IsPlatformSupported)
                docs = KnownFolders.DocumentsLibrary;
            else
                docs = KnownFolders.Documents;

            ShellContainer docsContainer = docs as ShellContainer;

            foreach (ShellObject so in docs)
            {
                RecurseDisplay(so);

                if (_backgroundWorker.CancellationPending)
                {
                    SetLabelButtonStatus(StartStopIndexerButton, "Start Search Indexer");
                    SetLabelButtonStatus(IndexerStatusLabel, "Click \"Start Search Indexer\" to run the indexer");
                    SetLabelButtonStatus(IndexerCurrentFileLabel, (_cancelReason == "powerSourceChanged") ?
                                "Indexing cancelled due to a change in power source" :
                                "Indexing cancelled by the user");

                    return;
                }

                Thread.Sleep(1000); // sleep a second to indicate indexing the file
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CapturePowerManagementEvents();
            GetPowerSettings();
        }

        // Get the current property values from PowerManager.
        // This method is called on startup.
        private void GetPowerSettings()
        {
            _settings.PowerPersonality = PowerManager.PowerPersonality.ToString();
            _settings.PowerSource = PowerManager.PowerSource.ToString();
            _settings.BatteryPresent = PowerManager.IsBatteryPresent;
            _settings.UpsPresent = PowerManager.IsUpsPresent;
            _settings.MonitorOn = PowerManager.IsMonitorOn;
            _settings.MonitorRequired = PowerManager.MonitorRequired;

            if (PowerManager.IsBatteryPresent)
            {
                _settings.BatteryShortTerm = PowerManager.IsBatteryShortTerm;
                _settings.BatteryLifePercent = PowerManager.BatteryLifePercent;

                BatteryState batteryState = PowerManager.GetCurrentBatteryState();

                string batteryStateStr =
                    $"ACOnline: {batteryState.AcOnline}{Environment.NewLine}Max Charge: {batteryState.MaxCharge} mWh{Environment.NewLine}Current Charge: {batteryState.CurrentCharge} mWh{Environment.NewLine}Charge Rate: {(batteryState.AcOnline ? "N/A" : batteryState.ChargeRate.ToString() + " mWh")} {Environment.NewLine}Estimated Time Remaining: {(batteryState.AcOnline ? "N/A" : batteryState.EstimatedTimeRemaining.ToString())}{Environment.NewLine}Suggested Critical Battery Charge: {batteryState.SuggestedCriticalBatteryCharge} mWh{Environment.NewLine}Suggested Battery Warning Charge: {batteryState.SuggestedBatteryWarningCharge} mWh{Environment.NewLine}";

                _settings.BatteryState = batteryStateStr;
            }
        }

        // Adds event handlers for PowerManager events.
        private void CapturePowerManagementEvents()
        {
            PowerManager.IsMonitorOnChanged += MonitorOnChanged;
            PowerManager.PowerPersonalityChanged += PowerPersonalityChanged;
            PowerManager.PowerSourceChanged += PowerSourceChanged;
            if (PowerManager.IsBatteryPresent)
            {
                PowerManager.BatteryLifePercentChanged += BatteryLifePercentChanged;

                // Set the label for the battery life
                SetLabelButtonStatus(batteryLifePercentLabel, $"{PowerManager.BatteryLifePercent.ToString()}%");
            }

            PowerManager.SystemBusyChanged += SystemBusyChanged;
        }

        // PowerManager event handlers.

        void MonitorOnChanged(object sender, EventArgs e)
        {
            _settings.MonitorOn = PowerManager.IsMonitorOn;
            AddEventMessage($"Monitor status changed (new status: {(PowerManager.IsMonitorOn ? "On" : "Off")})");
        }

        void PowerPersonalityChanged(object sender, EventArgs e)
        {
            _settings.PowerPersonality = PowerManager.PowerPersonality.ToString();
            AddEventMessage($"Power Personality changed (current setting: {PowerManager.PowerPersonality.ToString()})");
        }

        void PowerSourceChanged(object sender, EventArgs e)
        {
            _settings.PowerSource = PowerManager.PowerSource.ToString();
            AddEventMessage($"Power source changed (current source: {PowerManager.PowerSource.ToString()})");

            //
            if (_backgroundWorker.IsBusy)
            {
                if (PowerManager.PowerSource == PowerSource.Battery)
                {
                    // for now just stop
                    _cancelReason = "powerSourceChanged";
                    _backgroundWorker.CancelAsync();
                }
                else
                {
                    // If we are currently on AC or UPS and switch to UPS or AC, just ignore.
                }
            }
            else
            {
                if (PowerManager.PowerSource == PowerSource.Ac || PowerManager.PowerSource == PowerSource.Ups)
                {
                    SetLabelButtonStatus(IndexerStatusLabel, "Click \"Start Search Indexer\" to run the indexer");
                }
            }
        }

        void BatteryLifePercentChanged(object sender, EventArgs e)
        {
            _settings.BatteryLifePercent = PowerManager.BatteryLifePercent;
            AddEventMessage($"Battery life percent changed (new value: {PowerManager.BatteryLifePercent})");

            // Set the label for the battery life
            SetLabelButtonStatus(batteryLifePercentLabel, $"{PowerManager.BatteryLifePercent.ToString()}%");
        }

        // The event handler must use the window's Dispatcher
        // to update the UI directly. This is necessary because
        // the event handlers are invoked on a non-UI thread.
        void SystemBusyChanged(object sender, EventArgs e)
        {
            AddEventMessage($"System busy changed at {DateTime.Now.ToLongTimeString()}");
        }

        void AddEventMessage(string message)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal,
                (MethodInvoker)delegate
                {
                    ListBoxItem lbi = new ListBoxItem();
                    lbi.Content = message;
                    messagesListBox.Items.Add(lbi);
                    messagesListBox.ScrollIntoView(lbi);
                });
        }

        private void StartIndexer(object sender, RoutedEventArgs e)
        {
            if (_backgroundWorker.IsBusy && ((Button)sender).Content.ToString() == "Stop Indexer")
            {
                _cancelReason = "userCancelled";
                _backgroundWorker.CancelAsync();
                SetLabelButtonStatus(IndexerStatusLabel, "Click \"Start Search Indexer\" to run the indexer");
                return;
            }

            // If running on battery, don't start the indexer
            if (PowerManager.PowerSource != PowerSource.Battery)
            {
                _backgroundWorker.RunWorkerAsync();
                SetLabelButtonStatus(IndexerStatusLabel, "Indexer running....");
                SetLabelButtonStatus(StartStopIndexerButton, "Stop Indexer");
            }
            else
            {
                SetLabelButtonStatus(IndexerCurrentFileLabel, "Running on battery. Not starting the indexer");
            }
        }

        private void RecurseDisplay(ShellObject so)
        {
            if (_backgroundWorker.CancellationPending)
                return;

            SetLabelButtonStatus(IndexerCurrentFileLabel,
                $"Current {(so is ShellContainer ? "Folder" : "File")}: {so.ParsingName}");

            // Loop through this object's child items if it's a container
            ShellContainer container = so as ShellContainer;

            if (container != null)
            {
                foreach (ShellObject child in container)
                    RecurseDisplay(child);
            }
        }

        private void SetLabelButtonStatus(ContentControl control, string status)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal,
                (MethodInvoker)delegate
                {
                    control.Content = status;
                });
        }
    }
}
