using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using NetworkMonitor.Commands;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace NetworkMonitor.ViewModels
{
    public class DataMonitorViewModel : BaseViewModel
    {
        private double _dataUsed;
        private ICommand _buttonCommand;
        private ObservableCollection<NetworkInterface> _networkInterfaces;
        private NetworkInterface _selectedInterface;

        public double DataUsed
        {
            get => _dataUsed;
            set
            {
                _dataUsed = value; 
                OnPropertyChanged(nameof(DataUsed));
            }
        }

        public NetworkInterface SelectedInterface
        {
            get => _selectedInterface;
            set
            {
                _selectedInterface = value;
                OnPropertyChanged(nameof(SelectedInterface));
            }
        }

        public ObservableCollection<NetworkInterface> NetworkInterfaces
        {
            get => _networkInterfaces;
            set
            {
                _networkInterfaces = value;
                OnPropertyChanged(nameof(NetworkInterfaces));
            }
        }

        public ICommand ButtonCommand
        {
            get
            {
               return _buttonCommand ?? (_buttonCommand = new CommandHandler(param => StartMonitoring(),true));
            }
            
        }

        public EventHandler MonitoringEventHandler { get; set; }

        public DataMonitorViewModel()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            NetworkInterfaces = new ObservableCollection<NetworkInterface>(networkInterfaces);
            
        }

        public void StartMonitoring()
        {
            DataUsed = ConvertBytesToMegabytes(SelectedInterface.GetIPv4Statistics().BytesReceived);
            
            MonitoringEventHandler.Invoke(this, new EventArgs());
            var timer = new Timer();
            timer.Tick += TimerOnTick;
            timer.Interval = 1000;
            timer.Start();
        }

        private void TimerOnTick(object sender, EventArgs e)
        {
            var received = ConvertBytesToMegabytes(SelectedInterface.GetIPv4Statistics().BytesReceived);
            var sent = ConvertBytesToMegabytes(SelectedInterface.GetIPv4Statistics().BytesSent);
            DataUsed = received + sent;
            
            MonitoringEventHandler.Invoke(this, new EventArgs());
        }

        static double ConvertBytesToMegabytes(long bytes)
        {
            var mbs = (bytes / 1024f) / 1024f;
            return Math.Round(mbs, 2);
        }
    }
}
