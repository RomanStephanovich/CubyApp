using CUBY.Services.Interfaces;
using Syntec.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUBY.Services.Implementations
{
    public class SyntecCncClient : ISyntecCncClient
    {
        private readonly SyntecRemoteCNC _cnc;

        public SyntecCncClient(string ip, int timeout)
        {
            _cnc = new SyntecRemoteCNC(ip, timeout);
        }

        public short READ_information(out short axes, out string cncType, out short maxAxes, out string series, out string version, out string[] axisNames)
            => _cnc.READ_information(out axes, out cncType, out maxAxes, out series, out version, out axisNames);

        public short READ_status(out string emg, out string alarm, out int mode, out string run, out string auto, out string mdi, out string edit)
            => _cnc.READ_status(out emg, out alarm, out mode, out run, out auto, out mdi, out edit);
    }
}
