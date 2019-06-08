using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwinGameSDK;
using System.Threading.Tasks;

namespace MyGame
{
    class Menu
    {
        private bool _menuClicked,_changeClicked,_addClicked,_addTypeClicked,_newAction;
        private List<Action> _acts;
        private Action _action;
        private Planner planner;
        private List<ActType> _typeList;
        private int _menuX, _menuY,_menuHeight,_optionWidth, _menuButtonLength;
        public Menu(Planner _planner)
        { 
            _typeList = new List<ActType>();
            planner = _planner;
            AddActions();
            _menuX = 580;
            _menuY = 0;
            _menuHeight = 20;
            _newAction = true;
            _optionWidth = 150;
             _menuButtonLength = 60;
            _action=null;
        }
        private void AddActions()
        {
            _acts = new List<Action>();
            ChangeView _changeView = new ChangeView(planner);
            AddAct _addActivity = new AddAct(planner);
            AddType _addtype = new AddType(planner);
            _acts.Add(_changeView);
            _acts.Add(_addtype);
            _acts.Add(_addActivity);
        }
        //draws menu bar, if menu button clicked draws options, if action clicked action executed 
        public void Draw()
        {
            DrawMenuBar();
            if ((SwinGame.MouseClicked(MouseButton.LeftButton)) & (SwinGame.PointInRect(SwinGame.MousePosition(), _menuY, _menuX,_menuButtonLength, _menuHeight)))
            {
                _menuClicked = true;
            }
            if (_menuClicked)
            {
                DrawMenuOptions();
                ExecuteAction();
                //_menuClicked is false if area outside menu options is clicked menu, exits menu loop
                if ((SwinGame.MouseClicked(MouseButton.LeftButton)) & (SwinGame.PointInRect(SwinGame.MousePosition(), _menuY, _menuX - _menuHeight, _optionWidth * 2, _menuHeight * 3) != true))
                {
                    _menuClicked = false;
                }
            };
        }
        private void ExecuteAction()
        {
            if (_newAction)
            {
                _action = null;
                string id = "";
                _addTypeClicked = OptionClicked(3);
                if (_addTypeClicked) { id = "addtype"; }
                _addClicked = OptionClicked(2);
                if (_addClicked) { id = "addactivity"; }
                _changeClicked = OptionClicked(1);
                if (_changeClicked) { id = "changeview"; }
                _action = GetAction(id);
            }
            if (_action != null) { _newAction = !_action.Execute(); }
        }
        private Action GetAction(string id)
        {
            Action result = null;
            foreach (Action action in _acts)
            {             
                if (action.Id == id)
                {
                    result = action;
                }
            }
            return result;
        }
        private bool OptionClicked(int option) {
            bool result=false;         
            if ((SwinGame.MouseClicked(MouseButton.LeftButton)) & (SwinGame.PointInRect(SwinGame.MousePosition(), 0, _menuX-(_menuHeight*option), _optionWidth, _menuHeight)))
            {
                result = true;
            }
            return result;
        }    
        private  void DrawMenuOptions()
        {                
            int i = 1;
            string[] optionLabels=new string[] { "Change View", "Add New Activity", "Add New Type" };
            foreach (Action act in _acts) {
                int y = _menuX - i * _menuHeight;
                SwinGame.FillRectangle(Color.DimGray, 0,y, _optionWidth, _menuHeight);
                SwinGame.DrawText(optionLabels[i-1],Color.Black, _menuHeight - 10,y +5);
                i++;
            }                   
        }
        private  void DrawMenuBar()
        {
            int _menuBarLength=800;           
            SwinGame.FillRectangle(Color.DimGray, _menuY, _menuX, _menuBarLength, _menuHeight);//draws menu bar
            SwinGame.DrawRectangle(Color.Black, _menuY, _menuX, _menuButtonLength, _menuHeight);//draws menu button box
            SwinGame.DrawRectangle(Color.Black, _menuY, _menuX, _menuBarLength, _menuHeight);//draws menu bar
            SwinGame.DrawText("Menu", Color.Black, _menuY+10, _menuX+5);//draws menu button text
        }
        public List<ActType> ActTypes
        {
            get { return _typeList; }
        }
    }
}
