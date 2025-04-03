using CUBY.Services.Implementations;
using Syntec.Remote;
using System;
using System.Windows;

namespace CUBY.Gui
{
    public partial class MainWindow : Window
    {
        private SyntecRemoteCNC _cnc;

        public MainWindow()
        {
            InitializeComponent();
            IpBox.Text = "192.168.1.100";
            TimeoutBox.Text = "3000";
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ip = IpBox.Text;
                int timeout = int.Parse(TimeoutBox.Text);

                _cnc = new SyntecRemoteCNC(ip, timeout);
                Log($" Connected to {ip} with timeout {timeout}ms");
            }
            catch (Exception ex)
            {
                Log($" Connection error: {ex.Message}");
            }
        }

        private void GetInfo_Click(object sender, RoutedEventArgs e)
        {
            if (!EnsureConnected()) return;

            try
            {
                short axes, maxAxes;
                string cncType, version, series;
                string[] axisNames;

                short result = _cnc.READ_information(out axes, out cncType, out maxAxes, out series, out version, out axisNames);

                if (result == 0)
                {
                    Log($" Info → Type: {cncType}, Ver: {version}, Series: {series}, Axes: {axes}");
                    Log("Axis Names: " + string.Join(", ", axisNames ?? Array.Empty<string>()));
                }
                else
                {
                    Log($" Failed to get info. Error code: {result}");
                }
            }
            catch (Exception ex)
            {
                Log($" Exception: {ex.Message}");
            }
        }

        private void GetStatus_Click(object sender, RoutedEventArgs e)
        {
            if (!EnsureConnected()) return;

            try
            {
                string emg, alarm, run, auto, mdi, edit;
                int mode;

                short result = _cnc.READ_status(out emg, out alarm, out mode, out run, out auto, out mdi, out edit);

                if (result == 0)
                {
                    Log("Status:");
                    Log($"   EMG: {emg}, ALARM: {alarm}, MODE: {mode}");
                    Log($"   RUN: {run}, AUTO: {auto}, MDI: {mdi}, EDIT: {edit}");
                }
                else
                {
                    Log($" Failed to read status. Code: {result}");
                }
            }
            catch (Exception ex)
            {
                Log($" Exception while reading status: {ex.Message}");
            }
        }

        private bool EnsureConnected()
        {
            if (_cnc == null)
            {
                Log(" Not connected.");
                return false;
            }

            return true;
        }

        private void Log(string message)
        {
            OutputBox.Text += $"[{DateTime.Now:HH:mm:ss}] {message}\n";
        }
    }
}