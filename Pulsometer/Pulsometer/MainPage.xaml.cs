﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Tizen.Wearable.CircularUI.Forms;

namespace Pulsometer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : CirclePage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnActionButtonClicked(object sender, EventArgs e)
        {

        }
    }
}