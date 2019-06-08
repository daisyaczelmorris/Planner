using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    abstract class View
    {


        virtual public void Draw(int xpos, int width) {

        }
        virtual public void Draw() {
        }
        

        public void Save() {
        }

        public void ChangeView() {
        }
    }
}
