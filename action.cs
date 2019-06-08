using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    abstract class Action

    {
        private string _id;
        private bool _result;
        
        public Action(string id) {
            _id = id;
        }
        public abstract bool Execute();
        public string Id
        {
            get { return _id; }
        }

        public bool Result { get => _result; set => _result = value; }
    }
}
