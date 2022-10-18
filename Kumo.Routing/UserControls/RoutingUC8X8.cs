using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Mvvm.Native;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.ViewInfo;
using CheckButton.Properties;


namespace Kumo.Routing.UserControls
{
    public partial class RoutingUC8X8 : XtraUserControl
    {
        private Font NameFont { get; }
        private Font NumberFont { get; }
        private DevExpress.XtraEditors.CheckButton[] Inputs { get; }
        private DevExpress.XtraEditors.CheckButton[] Outputs { get; }
        private int SelectedInput { get; set; } = 0;
        private int SelectedOutput { get; set; } = 0;
        private StringFormat ButtonsStringFormat { get; }
        private SemaphoreSlim lockSlim;
        public RoutingUC8X8()
        {
            InitializeComponent();
            lockSlim = new SemaphoreSlim(1);
            ButtonsStringFormat = new StringFormat();
            ButtonsStringFormat.Alignment = StringAlignment.Center;
            ButtonsStringFormat.LineAlignment = StringAlignment.Center;
            FontFamily fontFamily = new FontFamily("Tahoma");
            NameFont = new Font(fontFamily, 16, FontStyle.Regular, GraphicsUnit.Pixel);
            NumberFont = new Font(fontFamily, 25, FontStyle.Regular, GraphicsUnit.Pixel);
            Inputs = new[] { ib1, ib2, ib3, ib4, ib5, ib6, ib7, ib8 };
            Outputs = new[] { ob1, ob2, ob3, ob4, ob5, ob6, ob7, ob8 };

            var all = Inputs.ToList();
            all.AddRange(Outputs);
            foreach (DevExpress.XtraEditors.CheckButton button in Inputs)
            {
                button.BackgroundImage = Resources.InputWithCircle110x137;
            }
            foreach (DevExpress.XtraEditors.CheckButton button in Outputs)
            {
                button.BackgroundImage = Resources.OutputWithCircle110x138;

            }
            foreach (DevExpress.XtraEditors.CheckButton button in all)
            {
                button.Font = NumberFont;
                button.Width = 110;
                button.Height = 138;
                button.LookAndFeel.UseDefaultLookAndFeel = false;
                button.Appearance.BackColor = Color.Transparent;
                button.Appearance.ForeColor = Color.Transparent;
                button.Appearance.BorderColor = Color.Transparent;
                button.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
                button.ShowFocusRectangle = DefaultBoolean.False;
                button.Appearance.Options.UseBackColor = true;
                button.Appearance.Options.UseBorderColor = true;
                button.Appearance.Options.UseForeColor = true;
            }

        }

        private void RoutingUC8X8_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                Inputs[i].Tag = (i);
                Outputs[i].Tag = (i);
                Outputs[i].Enabled =true;
                Outputs[i].ForeColor =  Color.White ;
            }
            tableLayoutPanel1.Paint += RoutingUC_Paint;
           

        }

        private void RoutingUC_Paint(object sender, PaintEventArgs e)
        {
            var numberOfSources = Inputs.Length;
            int lineSize = 3;
            for (int i = 0; i < numberOfSources; i++)
            {
                List<int> targets = new List<int>();
                var input = Inputs[i];
                Color arrowColor = Color.White;
            

                PointF p1 = new PointF(input.Location.X + input.Width / 2, input.Location.Y + input.Height - 3);
                for (int j = 0; j < targets.Count; j++)
                {
                    var output = Outputs[targets[j] - 1];
                    var p2 = new PointF(output.Location.X + output.Width / 2, output.Location.Y + 3);
                    if (!output.Enabled)
                    {
                        output.Appearance.BackColor = Color.Transparent;
                        output.Appearance.ForeColor = Color.White;
                        output.Appearance.BorderColor = Color.White;
                    }
                    else
                    {
                        //output.Appearance.ForeColor = KumoSettings.UseColorsForButtonsText ? arrowColor : Color.White;
                    }
                    //get the source port number that belong to 'portNum'
                    
                        using (Pen p = new Pen(arrowColor, lineSize))
                        using (SolidBrush circleBrush = new SolidBrush(arrowColor))
                        using (GraphicsPath capPath = new GraphicsPath())
                        {
                            e.Graphics.DrawLine(p, p1, p2);
                            if (3 > 0)
                            {
                                e.Graphics.FillCircle(circleBrush, p1.X, p1.Y, 3);
                                e.Graphics.FillCircle(circleBrush, p2.X, p2.Y, 3);
                            }
                        }
                    
                }

            }


        }

        private void InputButton_Paint(object sender, PaintEventArgs e)
        {
            DevExpress.XtraEditors.CheckButton btn = sender as DevExpress.XtraEditors.CheckButton;
            SimpleButtonViewInfo vi = btn.GetViewInfo() as SimpleButtonViewInfo;
            if (btn.Tag == null)
            {
                return;
            }
            using (GraphicsCache cache = new GraphicsCache(e.Graphics))
            {
                cache.DrawString(btn.Tag.ToString(), NameFont, btn.Appearance.GetForeBrush(cache),
                    new Rectangle(vi.Bounds.X, vi.Bounds.Y + 90, btn.Width, 20), ButtonsStringFormat);
                cache.DrawString((Array.IndexOf(Inputs, btn) + 1).ToString(), NumberFont, btn.Appearance.GetForeBrush(cache),
                    new Rectangle(vi.Bounds.X, vi.Bounds.Y + 10, btn.Width, 50), ButtonsStringFormat);
            }
        }

        private void OutputButton_Paint(object sender, PaintEventArgs e)
        {
            DevExpress.XtraEditors.CheckButton btn = sender as DevExpress.XtraEditors.CheckButton;
            SimpleButtonViewInfo vi = btn.GetViewInfo() as SimpleButtonViewInfo;
            if (btn.Tag == null)
            {
                return;
            }
            using (GraphicsCache cache = new GraphicsCache(e.Graphics))
            {
                cache.DrawString(btn.Tag.ToString(), NameFont, btn.Appearance.GetForeBrush(cache),
                    new Rectangle(vi.Bounds.X, vi.Bounds.Y + 30, btn.Width, 20), ButtonsStringFormat);
                cache.DrawString((Array.IndexOf(Outputs, btn) + 1).ToString(), NumberFont, btn.Appearance.GetForeBrush(cache),
                    new Rectangle(vi.Bounds.X, vi.Bounds.Y + 70, btn.Width, 50), ButtonsStringFormat);
            }
        }

        private async void InputButton_Click(object sender, EventArgs e)
        {
            try
            {
                await lockSlim.WaitAsync();
                var ib = (sender) as SimpleButton;
                SelectedInput = Inputs.IndexOf(b => b == ib) + 1;
                if (SelectedInput > 0 && SelectedOutput > 0)
                {
                }
            }
            finally
            {
                lockSlim.Release();
            }
        }

        private async void OutputButton_Click(object sender, EventArgs e)
        {
            try
            {
                await lockSlim.WaitAsync();
                var ib = (sender) as SimpleButton;
                SelectedOutput = Outputs.IndexOf(b => b == ib) + 1;
                if (SelectedInput > 0 && SelectedOutput > 0)
                {
                }
            }
            finally
            {
                lockSlim.Release();
            }
        }
     

        private async void OutputButton_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                await lockSlim.WaitAsync();
                var ib = (sender) as DevExpress.XtraEditors.CheckButton;
                SelectedOutput = Outputs.IndexOf(b => b == ib) + 1;
                SelectedInput = 0;
            }
            finally
            {
                lockSlim.Release();
            }
        }
    }
}
