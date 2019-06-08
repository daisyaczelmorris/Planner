using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    interface IIterator
    {
        View Next(Planner planner);
        View Prev(Planner planner);
    }

    class ViewIterator : IIterator
    {
        private int _viewNumber;
        public ViewIterator(){
         _viewNumber = 0;
         }
        public View Next(Planner planner)
        {
            View _view=planner.View;
            _viewNumber = _viewNumber + 1;
            string   _viewType = _view.GetType().ToString();
            switch (_viewType)
            {
                case "MyGame.Week":
                    _view = planner.Weeks.ElementAt(_viewNumber);
                    break;
                case "MyGame.Month":
                    _view = planner.Months.ElementAt(_viewNumber);
                    break;
                case "MyGame.Day":
                    _view = planner.Days.ElementAt(_viewNumber);
                    break;
            }
            return _view;
        }
       public View Prev(Planner planner)
        {
            View _view = planner.View;
            _viewNumber = _viewNumber - 1;
            string _viewType = _view.GetType().ToString();
            switch (_viewType)
            {
                case "MyGame.Week":
                    _view = planner.Weeks.ElementAt(_viewNumber);
                    break;
                case "MyGame.Month":
                    _view = planner.Months.ElementAt(_viewNumber);
                    break;
                case "MyGame.Day":
                    _view = planner.Days.ElementAt(_viewNumber);
                    break;
            }
            return _view;
        }
        public int ViewNumber {
            get { return _viewNumber; }
        }
    }
}
