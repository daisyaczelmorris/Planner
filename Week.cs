using System;
using System.Collections.Generic;
using System.Linq;
using SwinGameSDK;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    class Week : View
    {
        private List<Day> _daysInWeek;
        public Week() {
            _daysInWeek = new List<Day>();
        }
        public List<Day> Days{
             get { return _daysInWeek; }
         set { _daysInWeek =value; } 
        }

        public override void Draw() {

            int xpos = 50;
            int width = 80;
            foreach (Day d in _daysInWeek) {
                d.Draw(xpos, width);
                xpos = xpos + width+10;
            };
        }

    }
}
