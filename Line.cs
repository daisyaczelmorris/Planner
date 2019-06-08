using System;
using System.Collections.Generic;
using System.Linq;
using SwinGameSDK;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    class Line
    {
        private string _label;
        private int _y;
        private int _x;
        private string _input;
        public Line(string label, int y)
        {
            _input = "";
            _y = y;
            _label = label;
            _x = 30;
        }
        public void Draw()
        {
            SwinGame.FillRectangle(Color.White, _x + 320, _y, 75, 10);
            SwinGame.DrawText(_label, Color.Black, _x, _y);
            SwinGame.DrawText(_input, Color.Black, _x + 320, _y);
        }
        public String Input
        {
            get { return _input; }
            set { _input = value; }
        }
        public int Y
        {
            get { return _y; }
        }

    }
}
