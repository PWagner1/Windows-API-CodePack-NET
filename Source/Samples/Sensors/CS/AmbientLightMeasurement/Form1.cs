// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Sensors;
using Microsoft.WindowsAPICodePack.Dialogs;

using TD = Microsoft.WindowsAPICodePack.Dialogs.TaskDialog;

namespace AmbientLightMeasurement
{
    public partial class Form1 : Form
    {
        readonly Dictionary<Guid, ProgressBar> _sensorMap = new Dictionary<Guid, ProgressBar>();
        const int MaxIntensity = 200;

        public Form1()
        {
            InitializeComponent();

            SensorManager.SensorsChanged += SensorManager_SensorsChanged;

            PopulatePanel();
        }

        private void PopulatePanel()
        {
            try
            {
                SensorList<AmbientLightSensor> alsList = SensorManager.GetSensorsByTypeId<AmbientLightSensor>();

                panel.Controls.Clear();

                int ambientLightSensors = 0;
                foreach (AmbientLightSensor sensor in alsList)
                {
                    // Create a new progress bar to monitor light level.
                    ProgressBar pb = new ProgressBar();
                    pb.Width = 300;
                    pb.Height = 20;
                    pb.Top = 10 + 40 * ambientLightSensors;
                    pb.Left = 10;
                    pb.Maximum = MaxIntensity;

                    // Identify the control the bar represents.
                    Label label = new Label();
                    label.Text = $"SensorId = {sensor.SensorId}";
                    label.Top = pb.Top;
                    label.Left = pb.Right + 20;
                    label.Height = pb.Height;
                    label.Width = 300;

                    // Add controls to panel.
                    panel.Controls.AddRange(new Control[] { pb, label });

                    // Map sensor id to progress bar for lookup in data report handler.
                    _sensorMap[sensor.SensorId.Value] = pb;

                    // set intial progress bar value
                    sensor.TryUpdateData();
                    float current = sensor.CurrentLuminousIntensity.Intensity;
                    pb.Value = Math.Min((int)current, MaxIntensity);

                    // Set up automatc data report handling.
                    sensor.AutoUpdateDataReport = true;
                    sensor.DataReportChanged += DataReportChanged;

                    ambientLightSensors++;
                }

                if (ambientLightSensors == 0)
                {
                    Label label = new Label();
                    label.Text = @"No Sensors Found";
                    label.Top = 10;
                    label.Left = 10;
                    label.Height = 20;
                    label.Width = 300;
                    panel.Controls.Add(label);
                }
            }
            catch (SensorPlatformException)
            {
                // This exception will also be hit in the Shown message handler.
            }
        }

        void SensorManager_SensorsChanged(SensorsChangedEventArgs change)
        {
            // The sensors changed event comes in on a non-UI thread. 
            // Whip up an anonymous delegate to handle the UI update.
            BeginInvoke(new MethodInvoker(delegate
            {
                PopulatePanel();
            }));
        }

        void DataReportChanged(Sensor sender, EventArgs e)
        {
            AmbientLightSensor als = sender as AmbientLightSensor;

            // The data report update comes in on a non-UI thread. 
            // Whip up an anonymous delegate to handle the UI update.
            BeginInvoke(new MethodInvoker(delegate
            {
                // find the progress bar for this sensor
                ProgressBar pb = _sensorMap[sender.SensorId.Value];

                // report data (clamp value to progress bar maximum )
                float current = als.CurrentLuminousIntensity.Intensity;
                pb.Value = Math.Min((int)current, MaxIntensity);
            }));
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            try
            {
                // ask for sensor permission if needed
                SensorList<Sensor> sl = SensorManager.GetAllSensors();
                SensorManager.RequestPermission(this.Handle, true, sl);
            }
            catch (SensorPlatformException spe)
            {
                TD dialog = new TD();
                dialog.InstructionText = spe.Message;
                dialog.Text = "This application will now exit.";
                dialog.StandardButtons = TaskDialogStandardButtons.Close;
                dialog.Show();
                Application.Exit();
            }
        }



    }
}
