using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public abstract class AbstractCommand
    {
        protected string _name = "";

        protected AbstractCommand() { }

        protected bool CheckName(string name)
        {
            return _name.Equals(name);
        }

        public abstract void Run(string[] info);
    }
}