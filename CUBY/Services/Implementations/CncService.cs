using CUBY.Services.Interfaces;
using Syntec.Remote;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUBY.Services.Implementations
{
    public class CncService : ICncService
    {
        private readonly ISyntecCncClient _cnc;

        public CncService(ISyntecCncClient cnc)
        {
            _cnc = cnc;
        }

        public string GetInfo()
        {
            Log("Fetching machine information...");

            short axes;
            string cncType, version, series;
            short maxAxes;
            string[] axisNames;

            short result = _cnc.READ_information(out axes, out cncType, out maxAxes, out series, out version, out axisNames);

            switch (result)
            {
                case 0:
                    return Log($"Machine Type: {cncType}, Version: {version}, Axes: {axes}\nAxis Names: {string.Join(", ", axisNames)}");
                case -16:
                    return Log("Connection error: Socket error (-16)");
                case -1:
                    return Log("Controller is currently busy");
                default:
                    return Log($"Unknown error. Code: {result}");
            }
        }

        public string GetStatus()
        {
            Log("Reading machine status...");

            string emg, alarm, run, auto, mdi, edit;
            int mode;

            var result = _cnc.READ_status(out emg, out alarm, out mode, out run, out auto, out mdi, out edit);

            return result == 0
                ? Log($"Status → EMG: {emg}, ALARM: {alarm}, MODE: {mode}, RUN: {run}, AUTO: {auto}, MDI: {mdi}, EDIT: {edit}")
                : Log($"Failed to read status. Code: {result}");
        }

        private string Log(string message)
        {
            var timestamped = $"[{DateTime.Now:HH:mm:ss}] {message}";
            Console.WriteLine(timestamped);
            return timestamped;
        }
    }
}