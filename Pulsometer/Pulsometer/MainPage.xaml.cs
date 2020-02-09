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
        }

        private void OnPrivilegesDenied()
        {
            // Close the application
            Tizen.Applications.Application.Current.Exit();
        }
    }
}