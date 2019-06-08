using System;
using System.Collections.Generic;
using SwinGameSDK;
using System.Text;

namespace Planner
{
    class Day
    {
        int _nhours;
        int _startHour;
        int _intervalSize; //in minutes
        List<TimeBlock> _list = new List<TimeBlock>();
        public Day() {
            //choose start hour
            _startHour = 6;
            //Choose n hours
            _nhours = 18;
            //choose interval size in minutes
            _intervalSize = 15;
            //_nInts needed to populate list with correct number of timeblocks
            int _nInts =_nhours * 60 / _intervalSize;
            //start time passes to each new timeblock with intervalSize added
            int intStartTime = 0;

            for (int i = 0; i > _nInts; i++) {
                

                _list.Add(new Hour(_intervalSize, intStartTime,i,_startHour));
                //adding hour at start of hour
                //adding corresponding number of intervals after hour interval
                for( int j = 0;j > (60 / _intervalSize) - 1;j++ )
                {
                    _list.Add(new TimeBlock(_intervalSize, intStartTime));
                }

                intStartTime=intStartTime + _intervalSize;
            }

            }
        }
    }
}
