using System;
using System.Collections.Generic;
using System.Text;

namespace Planner
{
    class Activity : TimeBlock
    {
        private string _name;
        public Activity(string name,int length, int start) : base(length, start) {
            _name = name;

        }
    }
}
