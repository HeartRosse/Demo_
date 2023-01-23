using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF2022UserNNLib;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Calculations calculations = new Calculations();
            TimeSpan[] starttime = new TimeSpan[5];
            starttime[0] = new TimeSpan(10, 0, 0);
            starttime[1] = new TimeSpan(11, 0, 0);
            starttime[2] = new TimeSpan(15, 0, 0);
            starttime[3] = new TimeSpan(15, 30, 0);
            starttime[4] = new TimeSpan(16, 50, 0);

            int[] durations = new int[5];
            durations[0] = 60;
            durations[1] = 30;
            durations[2] = 10;
            durations[3] = 10;
            durations[4] = 40;

            TimeSpan beginworkingtimes = new TimeSpan(8, 0, 0);
            TimeSpan endworkingtimes = new TimeSpan(18, 0, 0);

            int consultationtime = 30;

            foreach (string st in calculations.AvailablePeriods(starttime, durations, beginworkingtimes, endworkingtimes, consultationtime))
            {
                Console.WriteLine(st);
            }
        }
    }
}
