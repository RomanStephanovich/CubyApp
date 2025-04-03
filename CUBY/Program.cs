using Syntec.OpenCNC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syntec.Remote;
using CUBY.Services.Implementations;
using CUBY.Services.Interfaces;


namespace CUBY
{
    class Program
    {
        static void Main()
        {
            try
            {
               
                ICncService cncService = new CncService(
                    new SyntecCncClient("192.168.1.100", 3000)
                );

                string info = cncService.GetInfo();
                Console.WriteLine(info);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error : {ex.Message}");
            }

            Console.WriteLine("Enter...");
            Console.ReadLine();
        }
    }
}


    
