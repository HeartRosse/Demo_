using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF2022UserNNLib
{
    public class Calculations
    {
        public string[] AvailablePeriods(TimeSpan[] StartTimes, int[] Durations, TimeSpan BeginWorkingTime, TimeSpan EndWorkingTime, int ConsultationTime)
        {
            int i = 0;
            TimeSpan workdur = EndWorkingTime - BeginWorkingTime;
            int a = Convert.ToInt32(workdur.Ticks / new TimeSpan(0, ConsultationTime, 0).Ticks);
            TimeSpan[] endtime = new TimeSpan[StartTimes.Length];
            string[] retst = new string[a];
            foreach (TimeSpan start in StartTimes)
            {
                endtime[i] = start + new TimeSpan(0, Durations[i], 0);
                retst[i] = endtime[i].ToString();
                i++;
            }
            string[] timeforconsult = new string[a];
            int o = 0;
            for (int j = 0; j < StartTimes.Length; j++)
            {
                while (BeginWorkingTime + new TimeSpan(0, ConsultationTime, 0) <= StartTimes[j])
                {
                    if (o < timeforconsult.Length)
                    {
                        timeforconsult[o] = BeginWorkingTime.ToString() + "-" + (BeginWorkingTime + new TimeSpan(0, ConsultationTime, 0)).ToString();
                        BeginWorkingTime += new TimeSpan(0, ConsultationTime, 0);
                        o++;
                    }

                }
                BeginWorkingTime = endtime[j];
            }
            while (BeginWorkingTime + new TimeSpan(0, ConsultationTime, 0) <= EndWorkingTime)
            {
                timeforconsult[o] = BeginWorkingTime.ToString() + "-" + (BeginWorkingTime + new TimeSpan(0, ConsultationTime, 0)).ToString();
                BeginWorkingTime += new TimeSpan(0, ConsultationTime, 0);
                o++;
            }

            return timeforconsult;
        }
    }
}
