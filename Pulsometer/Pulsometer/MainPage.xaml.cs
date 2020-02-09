﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Tizen.Wearable.CircularUI.Forms;
using Tizen.Security;
using Tizen.Sensor;

namespace Pulsometer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : CirclePage
    {
        private HeartRateMonitor monitor;
        private bool measuring = false;

        public MainPage()
        {
            InitializeComponent();
            CheckPrivileges();
        }     
        
        private void OnActionButtonClicked(object sender, EventArgs e)
        {
            if (measuring)
            {
                StopMeasurement();
            }
            else
            {
                StartMeasurement();
            }
        }

        private void CheckPrivileges()
        {
            // Check permission status (allow, deny, ask) to determinate action which has to be taken
            string privilege = "http://tizen.org.privilege/healthinfo";
            CheckResult result = PrivacyPrivilegeManager.CheckPermission(privilege);

            if (result == CheckResult.Allow)
            {
                OnPrivilegesGranted();
            }
            else if (result == CheckResult.Deny)
            {
                OnPrivilegesDenied();
            }
            else // the user must be asked about granting the privilege
            {
                PrivacyPrivilegeManager.GetResponseContext(privilege).TryGetTarget(out var context);

                if (context != null)
                {
                    context.ResponseFetched += (sender, e) =>
                    {
                        if (e.cause == CallCause.Answer && e.result == RequestResult.AllowForever)
                        {
                            OnPrivilegesGranted();
                        }
                        else
                        {
                            OnPrivilegesDenied();
                        }
                    };
                }
            }
        }

        private void OnPrivilegesGranted()
        {
            // Create an instance of the monitor
            monitor = new HeartRateMonitor();
            // specify frequency of the sensor data event by setting the interval value (in milliseconds)
            monitor.Interval = 1000;

            // stop the measurement when the application goes background
            MessagingCenter.Subscribe<Application>(this, "sleep", (sender) =>
            {
                if (measuring)
                {
                    StopMeasurement();
                }
            });
        }

        private void OnPrivilegesDenied()
        {
            // Close the application
            Tizen.Applications.Application.Current.Exit();
        }

        private void OnMonitorDataUpdated(object sender, HeartRateMonitorDataUpdatedEventArgs e)
        {
            // Update displayed value
            hrValue.Text = e.HeartRate > 0 ? e.HeartRate.ToString() : "0";
        }

        private void StartMeasurement()
        {
            monitor.DataUpdated += OnMonitorDataUpdated;
            monitor.Start();
            measuring = true;

            // Update the view
            actionButton.Text = "STOP";
            measuringIndicator.IsVisible = true;
        }

        private void StopMeasurement()
        {
            monitor.DataUpdated -= OnMonitorDataUpdated;
            monitor.Stop();
            measuring = false;

            // Update the view
            actionButton.Text = "MEASURE";
            measuringIndicator.IsVisible = false;
        }
    }
}