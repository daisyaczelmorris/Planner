using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwinGameSDK;
using System.Threading.Tasks;

namespace MyGame
{
    class Month:View
    {
        private List<Day> _daysInMonth;
        private string _month;
        public Month(string month) {
            _month = month;
            _daysInMonth = new List<Day>();

        }
        public List<Day> Days
        {
            get { return _daysInMonth; }
            set { _daysInMonth = value; }
        }
        public override void Draw()
        {
            SwinGame.DrawText(_month, Color.Black, 20, 20);
            int height, width, x, y;
            height = 100; width = 100; y = 40;x = 50;
            int i = 0;
            foreach (Day d in _daysInMonth)//draw each day
            {
                SwinGame.DrawRectangle(Color.Black, x, y, height, width);
                SwinGame.DrawText(d.Date.ToString("d"), Color.Black, x + 5, y + 5);
                foreach (TimeBlock t in d.TimeBlocks)
                {
                    CheckLabel(x, y, t);
                }
                x = x + width;
                    if (i == 6 || i == 13 || i == 20 || i == 27) { y = y + height; x = 50; }
                    i++;              
            }
        }

        private static void CheckLabel(int x, int y, TimeBlock t)
        {
            if (t.Act != null)//if timeblock in day has activity
            {
                if (t.Act.Start == t.StartTime.ToString("HH:mm"))//if activity start = timeblock start
                {
                    int count = 1;
                    SwinGame.DrawText(t.Act.Label, Color.Black, x + 15, y + (count * 15));//draw activity label 
                    count++;
                }
            }
        }
    }
}
