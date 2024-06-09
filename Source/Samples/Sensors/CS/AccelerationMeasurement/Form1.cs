// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Sensors;
using Microsoft.WindowsAPICodePack.Dialogs;

using TD = Microsoft.WindowsAPICodePack.Dialogs.TaskDialog;

namespace AccelerationMeasurement
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            SensorManager.SensorsChanged += SensorManager_SensorsChanged;

            HookUpAccelerometer();
        }

        void SensorManager_SensorsChanged(SensorsChangedEventArgs change)
        {
            // The sensors changed event comes in on a non-UI thread. 
            // Whip up an anonymous delegate to handle the UI update.
            BeginInvoke(new MethodInvoker(delegate
            {
                HookUpAccelerometer();
            }));
        }

        private void HookUpAccelerometer()
        {
            try
            {
                SensorList<Accelerometer3D> sl = SensorManager.GetSensorsByTypeId<Accelerometer3D>();
                if (sl.Count > 0)
                {
                    Accelerometer3D accel = sl[0];
                    accel.AutoUpdateDataReport = true;
                    accel.DataReportChanged += DataReport_Changed;
                }

                availabilityLabel.Text = $@"Accelerometers available = {sl.Count}";
            }
            catch (SensorPlatformException)
            {
                // This exception will also be hit in the Shown message handler.
            }
        }

        void DataReport_Changed(Sensor sender, EventArgs e)
        {
            // The data report update comes in on a non-UI thread. 
            // Whip up an anonymous delegate to handle the UI update.
            BeginInvoke(new MethodInvoker(delegate
            {
                Accelerometer3D accel = (Accelerometer3D)sender;

                if (accel != null)
                {
                    accelX.Acceleration = accel.CurrentAcceleration[AccelerationAxis.XAxis];
                    accelY.Acceleration = accel.CurrentAcceleration[AccelerationAxis.YAxis];
                    accelZ.Acceleration = accel.CurrentAcceleration[AccelerationAxis.ZAxis];
                }
            }));
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            try
            {
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
