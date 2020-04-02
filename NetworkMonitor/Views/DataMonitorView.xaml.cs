using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NetworkMonitor.ViewModels;
using Brush = System.Drawing.Brush;
using Color = System.Drawing.Color;

namespace NetworkMonitor.Views
{
    /// <summary>
    /// Interaction logic for DataMonitorView.xaml
    /// </summary>
    public partial class DataMonitorView : UserControl
    {
        public DataMonitorView()
        {
            InitializeComponent();
            ((DataMonitorViewModel)DataContext).MonitoringEventHandler += MonitoringEventHandler;
           
        }

        private void MonitoringEventHandler(object sender, EventArgs e)
        {
            Font font = new Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Regular, GraphicsUnit.Pixel);
            Brush brush = new SolidBrush(Color.White);
            Bitmap bitmapText = new Bitmap(16, 16);
            Graphics g = System.Drawing.Graphics.FromImage(bitmapText);

            IntPtr icon;

            g.Clear(Color.Transparent);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            g.DrawString(((DataMonitorViewModel)DataContext).DataUsed.ToString("##.##"), font, brush, 0,0 );
            myNotifyIcon.Icon?.Dispose();
            icon = (bitmapText.GetHicon());
            myNotifyIcon.Icon = Icon.FromHandle(icon);
        }
    }
}
