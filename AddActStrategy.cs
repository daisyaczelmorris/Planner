using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwinGameSDK;
using System.Threading.Tasks;

namespace MyGame
{
    interface IAddAct
    {
         bool AddActivity();
         void AddActivity(string day,string time);
       // public abstract void AddActivity(string day);
    }
    class AddAct : Action, IAddAct
    {
        private   AddType _addType;
        private   Activity _activity;
        private    Planner _planner;
        private string[] _labels, _details;
        private bool _exitActivity,_addTypeClicked;
        private int _x, _y;
        private List<Line> _lines;

        internal Activity Activity { get => _activity; set => _activity = value; }

        public AddAct(Planner planner) : base("addactivity") {
            _planner = planner;
            Activity = new Activity();
            _addType = new AddType(_planner);
            _lines = new List<Line>();
            _details = new string[4];
            _labels = new string[] { "Enter Activity Date dd/mm/yyyy", "Enter Activity Start Time \"00:00\"", "Enter Activity End Time \"00:00\"", "Enter activity name", };
            _x = 30;
            _y = 75;
            int y = _y;
            for (int i = 0; i < 4; i++)
            {
                _lines.Add(new Line(_labels[i], y));
                y = y + 15;
            }
        }
        private int AddLine(int count, Font font, string message)
        {
            {
                _details[count] = message;
                _lines.ElementAt(count).Input = message;
                count++;
                Input.EndReadingText();
                if (count < 4) { Input.StartReadingText(Color.Black, 100, font, _x + 320, _lines.ElementAt(count).Y); }
                if (count == 4) { count++; }
            }
            return count;
        }
        // when activity added is true adds activity to time blocks in day based on start and end time of activty 
        public override bool Execute() {
            
            bool _activityAdded = AddActivity();
            if (_activityAdded)
            {
                _planner.ActCount++;
                foreach (Day day in _planner.Days)
                {
                    
                    if (day.Date.ToString("dd-MM-yyyy") == Activity.Date.ToString("dd-MM-yyyy"))
                    {
                        DateTime _startindex, _endindex;
                        FindIndexes(day, out _startindex, out _endindex,_activity);
                        foreach (TimeBlock timeblock in day.TimeBlocks)
                            AddActToDay(_startindex, _endindex, timeblock,_activity);
                    }
                }
                return false; ;
            }
            if (_exitActivity) {return false; }
            return true;
        }
        //finds the start and end time of activity
        public void FindIndexes(Day day, out DateTime _startindex, out DateTime _endindex, Activity act)
        {
            string _time = act.Start;
            string[] _times = new string[2];  //create array to hold just minutes and just hours without ":"
            _times = _time.Split(':');
            string _hour = _times[0];
            string _min = _times[1];
            int _h = int.Parse(_hour); //cast from string to int 
            int _m = int.Parse(_min);
            _startindex = new DateTime(day.Date.Year, day.Date.Month, day.Date.Day, _h, _m, 00);
            string _time1 =act.End;
            string[] _times1 = new string[2];
            _times1 = _time1.Split(':');
            string _hour1 = _times1[0];
            string _min1 = _times1[1];
            int _h1 = int.Parse(_hour1);
            int _m1 = int.Parse(_min1);
            _endindex = new DateTime(day.Date.Year, day.Date.Month, day.Date.Day, _h1, _m1, 00);
        }
        //adds activity to block if its with its time
        public void AddActToDay(DateTime _startindex, DateTime _endindex, TimeBlock timeblock,Activity act)
        {
            DateTime _blockEnd = timeblock.StartTime.AddMinutes(60/_planner.Days.ElementAt(0).IntervalSize);
            if ((timeblock.StartTime >= _startindex) & (_blockEnd <= _endindex))
            {
                timeblock.Act = act;
            }
        }
        //reads text into deatails line by line as user clicks enter, up to 4 lines then allows user to select type
        //count increments when user adds detail when count ==5 then all details are added to activity & box closes
        public bool AddActivity()
        {
            int count = 0;
            Activity = new Activity();//refresh the activity
            Font font = Text.LoadFont("Arial", 12);
            Input.StartReadingText(Color.Black, 100, font, _x + 320, _lines.ElementAt(0).Y);
            while (SwinGame.WindowCloseRequested() == false & count < 6)///when count =6 all info has been added note: there is no data checking at this point
            {
                SwinGame.ProcessEvents();
                SwinGame.ClearScreen(Color.White);
                DrawActivityBox();
                count = ReadInput(count, font);
                if ((SwinGame.MouseClicked(MouseButton.LeftButton)) & (SwinGame.PointInRect(SwinGame.MousePosition(), _x+420, _y-20, 50, 10)))//if exit clicked empty lines return false (act not added)
                {
                    _exitActivity = true;     //this tells execute to tell menu that the action is done 
                    Input.EndReadingText();
                    foreach (Line l in _lines) { l.Input = ""; }
                    return false; //this tells execute that an activity was not addedd
                }              
                SwinGame.RefreshScreen(60);
            }
            AddDetailsToActivity();
            return true;
        }
        public Activity AddDetailsToActivity(DateTime Date,string start,string end,string name,ActType actType) {
            Activity.Date = Date;
            Activity.Label = name;
            Activity.Start = start;
            Activity.End = end;
            Activity.ActType = actType;
            return _activity;

        }
        private void AddDetailsToActivity()
        {
            foreach (Line l in _lines) { l.Input = ""; }//lines.input cleared
            Activity.Date = DateTime.Parse(_details[0]);//details added to _acvtity
            Activity.Start = _details[1];
            Activity.End = _details[2];
            Activity.Label = _details[3];
        }
        private void DrawActivityBox()
        {
            SwinGame.FillRectangle(Color.LightGray, _x-20, _y-25, 500, 200);
            foreach (Line l in _lines)
            {
                l.Draw();
            }
            SwinGame.DrawText("Add Actvity", Color.Black, _x, _y-20);
            SwinGame.DrawText("Exit", Color.Black, _x+420, _y-20);
            SwinGame.DrawText("Choose activity type", Color.Black, _x, _y+60);
            SwinGame.DrawText("Add new Activity type", Color.Black, _x+170,_y+60);
            int x = 30;
            foreach (ActType _actType in _planner.Menu.ActTypes)
            {
                SwinGame.FillRectangle(_actType.Color, x, _y+75, 20, 20);
                SwinGame.DrawText(_actType.TypeLabel, Color.Black, x - 15, _y+100);
                x += 60;
            }
        }
        //readlines until count =4
        private int ReadInput(int count, Font font)
        {
            string message = Input.TextReadAsASCII();
            if (SwinGame.KeyTyped(KeyCode.ReturnKey) && count < 4)//when enter pressed count increments, this is used to keep track of lines added
                count = AddLine(count, font, message);//add previous line to _details and Input, starts reading next line
            count = AddColor(count);
            return count;
        }
        //if count ==4 allow user to add color
        private int AddColor(int count)
        {
            if (count > 4) //when count is five user can select activitytype or add a new one
            {
                if (SwinGame.MouseClicked(MouseButton.LeftButton) && SwinGame.PointInRect(SwinGame.MousePosition(), 200, 135, 60, 15))
                {
                    _addTypeClicked = true;
                }
                if (_addTypeClicked == true)
                {
                    Input.EndReadingText();
                    _addTypeClicked = _addType.Execute();
                }
                Activity.ActType = ChooseType();//checks for user selectiton of type add ads it
                if (Activity.ActType != null) { count++; } //once added all info is added and loop ends
            }
            return count;
        }
      
        //needs to be worked on
        public void AddActivity(string day, string time)//want to use this to add from day
        {
            
        }
        //used to let user choose type/color
        public ActType ChooseType()
        {
            int x = 60;
            int typeCount = 0;
            ActType _actType = null;
            foreach (ActType type in _planner.Menu.ActTypes)
            {
                if ((SwinGame.MouseClicked(MouseButton.LeftButton) && SwinGame.PointInRect(SwinGame.MousePosition(), (typeCount * x)+_x, _y+75, 20, 20)))
                {
                    _actType = type;
                }
                typeCount++;
            }
            return _actType;
        }
        //draws all lines
      
    }
}
