using System;
using System.Collections.Generic;
using System.Linq;
using SwinGameSDK;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    class AddType : Action
    {
        private ActType _type;
        private int _x, _y, _width, _height, _textX, _textY, _boxX;
        private bool _exitType, _nameDone, _colorDone;
        private Planner planner;
       
        private string _name, _typeName;
        private Color[] _possibleColors;
        private Color _color;
        public AddType(Planner _planner) : base("addtype")
        {
            _x = 10; _y = 50; _width = 525; _height = 200;
            _textX = 30 + 10; _textY = _y + 5; _boxX = _textX + 170;
            planner = _planner;
            _possibleColors = new Color[] {Color.AliceBlue,Color.BlueViolet,Color.DarkGreen,Color.DarkMagenta,Color.DarkRed,Color.OrangeRed, Color.DarkOrange,Color.Orange };
            _typeName = "";
        }      
        public override bool Execute()
        {
            //if type returned then add to menu typelist return false means "we dont need add type action any more"
            _type = TypeBox(); 
            if (_type != null)
            {
                planner.Menu.ActTypes.Add(_type);
                _type = null;
                _typeName = "";              
                return false;//returns false becuase type added
            }
            else if(_type== null &_exitType==true) { _typeName = ""; _exitType = false; return false; }//returns false becuase exit clicked
            return true;
       }
        public void DrawTypeBox()
        {
           

            SwinGame.FillRectangle(Color.LightGray, _x, _y, _width, _height);
            //draw text box, label, text input
            SwinGame.FillRectangle(Color.White, _boxX, _textY, _width/7, _height/20);
            SwinGame.DrawText("Type Name", Color.Black, _textX, _textY);
            SwinGame.DrawText(_typeName, Color.Black, _boxX, _textY);
            //draw other labels
            SwinGame.DrawText("Exit", Color.Black, _width-50, _textY);
            SwinGame.DrawText("Type Colour",Color.Black,_textX,_textY+15);
            //draw possible type colors
            int colorX = 30;
            foreach (Color color in _possibleColors) {
                SwinGame.FillRectangle(color, colorX, 85, 20, 20);
                colorX = colorX + 30;
            }                 
        }
        public ActType TypeBox()
        {
           
                 
            Font font = Text.LoadFont("Arial", 12);
            Input.StartReadingText(Color.Black, 100, font, _boxX, _y);
            while (_exitType == false & SwinGame.WindowCloseRequested() == false)
            {
                SwinGame.ProcessEvents();
                SwinGame.ClearScreen(Color.White);
                DrawTypeBox();
                _name = Input.TextReadAsASCII();
                CheckExit();
                CheckNameDone();//enter pressed
                ChooseColor();//color square clicked, only if _nameDone = true
                if (_name != null && _color != null && _nameDone && _colorDone)
                {
                    _colorDone = false;
                    _nameDone = false;                   
                    Input.EndReadingText();
                    return new ActType(_color, _name);
                }
                SwinGame.RefreshScreen(60);
            }
            return null;
        }
        private void CheckNameDone()
        {
            if (SwinGame.KeyTyped(KeyCode.ReturnKey) == true)
            {
                _nameDone = true;
                _typeName = _name;
                Input.EndReadingText();
            }
        }

        private void CheckExit()
        {
            if ((SwinGame.MouseClicked(MouseButton.LeftButton)) & (SwinGame.PointInRect(SwinGame.MousePosition(), _width-50,_textY, 50, 10)))
            {
                _exitType = true;
                Input.EndReadingText();

            }
        }
        private void ChooseColor()
        {
            if (_nameDone)
            {
                int x = 30;
                foreach (Color color in _possibleColors)
                {
                    if ((SwinGame.MouseClicked(MouseButton.LeftButton)) & (SwinGame.PointInRect(SwinGame.MousePosition(), x, 85, 20, 20)))
                    {
                        _color = color;
                        _colorDone = true;

                    }
                    x = x + 30;
                }             
            }
        }
    }
}
