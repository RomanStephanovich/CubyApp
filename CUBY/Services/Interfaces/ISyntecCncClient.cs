using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUBY.Services.Interfaces
{
    public interface ISyntecCncClient
    {
        short READ_information(out short axes, out string cncType, out short maxAxes, out string series, out string version, out string[] axisNames);
        short READ_status(out string emg, out string alarm, out int mode, out string run, out string auto, out string mdi, out string edit);
    }
}
