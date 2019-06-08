using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwinGameSDK;
using System.Threading.Tasks;

namespace MyGame
{
    class ChangeView : Action
    {
        private bool _result;
        private Planner _planner;
        private int _x, _y, _height, _width;
        public ChangeView(Planner planner) : base("changeview")
        {
            _planner = planner;
            _x = 150; _y = 560; _height = 20; _width = 120;
        }
        //draws view options, changes view based on user clicks, stops drawing optionsif clicked outside of
        public override  bool Execute()
        {
            //_result = true tells menu change view is not finished
            _result = true;
            //draw view options
            DrawOptions();
            // if view option clicked, planner.ViewNumber is reset to zero, the view is changed to selected view zero element, and result=false tells menu that change view is complete
            CheckUserClicks();
            return _result;
        }
        private void CheckUserClicks()
        {
            if ((SwinGame.MouseClicked(MouseButton.LeftButton)) & (SwinGame.PointInRect(SwinGame.MousePosition(), 120, _y, _width, _height)))
            {
                _planner.View = _planner.Days.ElementAt(0);
                _result = false;
            };
            if ((SwinGame.MouseClicked(MouseButton.LeftButton)) & (SwinGame.PointInRect(SwinGame.MousePosition(), 120, _y-_height, _width, _height)))
            {
                _planner.View = _planner.Weeks.ElementAt(0);
                _result = false;
            };
            if ((SwinGame.MouseClicked(MouseButton.LeftButton)) & (SwinGame.PointInRect(SwinGame.MousePosition(), 120, _y-_height*2, _width, _height)))
            {
                _planner.View = _planner.Months.ElementAt(0);
                _result = false;
            };
            if ((SwinGame.MouseClicked(MouseButton.LeftButton)) & !((SwinGame.PointInRect(SwinGame.MousePosition(), 120, 520, 120, 60)) || (SwinGame.PointInRect(SwinGame.MousePosition(), 0, 560, 120, 40))))
            {
                _result = false;
            }
        }
        private  void DrawOptions()
        {
            SwinGame.FillRectangle(Color.DimGray, _x, _y, _width, _height);
            SwinGame.DrawText("day", Color.Black, _x+10, _y+5);
            SwinGame.FillRectangle(Color.DimGray, _x, _y-_height, _width, _height);
            SwinGame.DrawText("week", Color.Black, _x+10, _y-_height+5);
            SwinGame.FillRectangle(Color.DimGray, _x, _y-_height*2, _width, _height);
            SwinGame.DrawText("month", Color.Black, _x+10, _y-(_height*2)+5);
        }      
    }
}
