using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraEditors;

namespace Kumo.Routing
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            WindowsFormsSettings.LoadApplicationSettings();
            WindowsFormsSettings.SetDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            UserLookAndFeel.Default.SkinName = "Office 2019 Black";
            Skin skin = FormSkins.GetSkin(UserLookAndFeel.Default.ActiveLookAndFeel);
            SkinElement element = skin[FormSkins.SkinFormCaption];
            element.Color.FontSize = 14;
            var form = new MainForm();
            form.ShowInTaskbar = true;
            Application.Run(form);
        }
    }
}
