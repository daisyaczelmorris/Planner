using System;
using System.Collections.Generic;
using System.Text;

namespace Planner
{
    class Hour:TimeBlock
    {
        private string _label;

        public Hour(int start, int length, int i, int firstHour) : base(start, length) {
            if (i < 13) { int h = i + firstHour; _label=h+"am" ; }
            else { _label = i - 12; }
        }
    }
}
