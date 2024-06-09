//Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.ComponentModel;

namespace Microsoft.WindowsAPICodePack.Samples.PowerMgmtDemoApp
{
    internal class MyPowerSettings : INotifyPropertyChanged
    {
        private string _powerPersonality;
        private string _powerSource;
        private bool _batteryPresent;
        private bool _upsPresent;
        private bool _monitorOn;
        private bool _batteryShortTerm;
        private int _batteryLifePercent;
        private string _batteryStateAcOnline;
        private bool _monitorRequired;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public string PowerPersonality
        {
            get => _powerPersonality;
            set
            {
                if (_powerPersonality != value)
                {
                    _powerPersonality = value;
                    OnPropertyChanged("PowerPersonality");
                }
            }
        }

        public string PowerSource
        {
            get => _powerSource;
            set
            {
                if (_powerSource != value)
                {
                    _powerSource = value;
                    OnPropertyChanged("PowerSource");
                }
            }
        }
        public bool BatteryPresent
        {
            get => _batteryPresent;
            set
            {
                if (_batteryPresent != value)
                {
                    _batteryPresent = value;
                    OnPropertyChanged("BatteryPresent");
                }
            }
        }
        public bool UpsPresent
        {
            get => _upsPresent;
            set
            {
                if (_upsPresent != value)
                {
                    _upsPresent = value;
                    OnPropertyChanged("UPSPresent");
                }
            }
        }

        public bool MonitorOn
        {
            get => _monitorOn;
            set
            {
                if (_monitorOn != value)
                {
                    _monitorOn = value;
                    OnPropertyChanged("MonitorOn");
                }
            }
        }
        public bool BatteryShortTerm
        {
            get => _batteryShortTerm;
            set
            {
                if (_batteryShortTerm != value)
                {
                    _batteryShortTerm = value;
                    OnPropertyChanged("BatteryShortTerm");
                }
            }
        }
        public int BatteryLifePercent
        {
            get => _batteryLifePercent;
            set
            {
                if (_batteryLifePercent != value)
                {
                    _batteryLifePercent = value;
                    OnPropertyChanged("BatteryLifePercent");
                }
            }
        }
        public String BatteryState
        {
            get => _batteryStateAcOnline;
            set
            {
                if (_batteryStateAcOnline != value)
                {
                    _batteryStateAcOnline = value;
                    OnPropertyChanged("BatteryState");
                }
            }
        }
        public bool MonitorRequired
        {
            get => _monitorRequired;
            set
            {
                if (_monitorRequired != value)
                {
                    _monitorRequired = value;
                    OnPropertyChanged("MonitorRequired");
                }
            }
        }

        // Create the OnPropertyChanged method to raise the event

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
