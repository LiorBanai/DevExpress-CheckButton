using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.XtraBars;
using DevExpress.XtraBars.ToolbarForm;
using Kumo.Routing.UserControls;

using Microsoft.Extensions.Logging;

namespace Kumo.Routing
{
    public partial class MainForm : ToolbarForm
    {
        private ILogger<MainForm> Logger { get; }


        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RoutingUC8X8 uc = new RoutingUC8X8();
            this.Controls.Add(uc);
            uc.Dock = DockStyle.Fill;

        }

        private void btsiDarkMode_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item is BarToggleSwitchItem toggle)
            {
                UserLookAndFeel defaultLF = UserLookAndFeel.Default;
                defaultLF.SkinName = toggle.Checked ? "Office 2019 Black" : "DevExpress Style";

            }
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Settings.ApplicationPosition.Location = Location;
            //Settings.ApplicationPosition.Size = Size;
            //Settings.ApplicationPosition.WindowState = WindowState;
            //Settings.Save();
        }

    }
}
