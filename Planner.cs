using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.IO;
using SwinGameSDK;

using System.Threading.Tasks;

namespace MyGame
{
    class Planner
    {
        private List<Day> _days;
        private Menu _menu;
        private List<Week> _weeks;
        private List<Month> _months;
        private int _actCount;
        private View _view;
        private ViewFactory _viewFactory;
        private ViewIterator _viewIterator;
        //create days, add days to weeks and months with normal view factory. Create menu, set default view.
        public Planner() {
            _actCount = 0;
            _viewFactory = new NormalViewFactory();
            _viewIterator = new ViewIterator();
            _days = _viewFactory.CreateDays();
            _weeks = _viewFactory.CreateWeeks(_days); ;
            _months = _viewFactory.CreateMonths(_days);
            _menu = new Menu(this);
            _view = _weeks.ElementAt(0);
        }
        //draw menu and _view, allow to go to next or prev view
        public void Draw()
        {
            //draw next & prev if _viewNumber not zero
            SwinGame.DrawText("next", Color.Black, 750, 40);
            if (_viewIterator.ViewNumber != 0) { SwinGame.DrawText("prev", Color.Black, 10, 40); }
            //next clicked changes _view to next element in the list based on _view type
            if ((SwinGame.MouseClicked(MouseButton.LeftButton)) & (SwinGame.PointInRect(SwinGame.MousePosition(), 750, 40, 30, 20)))
            {
                _view = _viewIterator.Next(this);
            }
            //prev clicked changes _view to last element in the list based on _view type
            if ((SwinGame.MouseClicked(MouseButton.LeftButton)) & (SwinGame.PointInRect(SwinGame.MousePosition(), 0, 40, 30, 20)) & _viewIterator.ViewNumber != 0)
            {
                _view = _viewIterator.Prev(this);
            }
            //tell _view to draw, tell _menu to draw
            _view.Draw();
            _menu.Draw();
            
        }
        public void SaveButton(string fileName) {
            SwinGame.DrawText("save", Color.Black,700,585);
            SwinGame.DrawRectangle(Color.Black, 695, 580, 50, 20);
            if (SwinGame.MouseClicked(MouseButton.LeftButton) && SwinGame.PointInRect(SwinGame.MousePosition(), 695, 580, 50, 20)) {
                Save(fileName);
            }
        }
        public void LoadButton(string fileName)
        {
            SwinGame.DrawText("load", Color.Black, 750, 585);
            SwinGame.DrawRectangle(Color.Black, 745, 580, 65, 20);
            if (SwinGame.MouseClicked(MouseButton.LeftButton) && SwinGame.PointInRect(SwinGame.MousePosition(), 745, 580, 65, 20))
            {
                Load(fileName);
            }

        }
        public void Save(string filename) {
            StreamWriter writer = new StreamWriter(filename);
            writer.WriteLine(_days.ElementAt(0).Date);
            writer.WriteLine(_menu.ActTypes.Count);
            foreach (ActType _acttype in _menu.ActTypes) {
                writer.WriteLine(_acttype.Color.ToArgb());
                writer.WriteLine(_acttype.TypeLabel);
            }
            writer.WriteLine(_actCount);
             foreach (Day day in _days) {
            
                day.Save(writer);
              }
            writer.Close();
        }
        public void Load(string filename)
        {
            this.Menu = new Menu(this);
           
            StreamReader reader = new StreamReader(filename);
            
            DateTime date = DateTime.Parse(reader.ReadLine());
           
            int typeNum = reader.ReadInteger();                  
            Color color;
            string label;
            for (int i = 0; i < typeNum; i++)
            {
                
                color = Color.FromArgb(reader.ReadInteger());
                label = reader.ReadLine();
                
                this.Menu.ActTypes.Add(new ActType(color,label));
            }
            

            int actNum =reader.ReadInteger();
            
          
            Activity[] activitys = new Activity[actNum];
            for (int i = 0; i < actNum; i++) 
            {
                AddAct addAct = new AddAct(this);
                string sDate = reader.ReadLine();
                DateTime dDate = DateTime.Parse(sDate);
                string start = reader.ReadLine();
                string end = reader.ReadLine();
                string name = reader.ReadLine();
                string typeName = reader.ReadLine();
                Color typeColor = Color.FromArgb(reader.ReadInteger());
                ActType actType = new ActType(typeColor, typeName);
                activitys[i] = addAct.AddDetailsToActivity(dDate, start, end, name, actType);
            }

            this.Days = _viewFactory.LoadDays(date);
            this.Weeks = _viewFactory.CreateWeeks(_days);
            this.Months = _viewFactory.CreateMonths(_days);
            this._actCount = actNum;
            foreach (Activity activity in activitys)
            {
                AddAct addAct = new AddAct(this);
                DateTime _startindex, _endindex;
                foreach (Day day in _days)
                {
                    foreach (Activity act in activitys)
                    {
                        if ((day.Date.ToString("dd-MM-yyyy") == act.Date.ToString("dd-MM-yyyy")))
                        {
                            addAct.FindIndexes(day, out _startindex, out _endindex,act);
                            foreach (TimeBlock timeblock in day.TimeBlocks)
                                addAct.AddActToDay(_startindex, _endindex, timeblock,act);
                        }
                    }
                }
            }   
           
            _view = _weeks.ElementAt(0);
            
            reader.Close();


        }

        public List<Day> Days{
            get { return _days; }
            set { _days=value; } 
        }
        public List<Month> Months
        {
            get { return _months; }
            set { _months = value; }
        }
        public List<Week> Weeks
        {
            get { return _weeks; }
            set { _weeks = value; }
        }
        public View View {
            set { _view = value; }
            get { return _view; }
        }
        public Menu Menu {
           get { return _menu; }
            set { _menu = value; }
        }

        public int ActCount { get => _actCount; set => _actCount = value; }
    }
}
