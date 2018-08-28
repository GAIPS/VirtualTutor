using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BubbleSystem
{
    public class BubbleSystemCommands : Singleton<BubbleSystemCommands>
    {
        private List<AbstractCommand> commands = new List<AbstractCommand>();

        private void Awake()
        {
            commands = ReflectiveEnumerator.GetEnumerableOfType<AbstractCommand>().ToList();
        }

        public void Run(string[] info)
        {
            foreach (AbstractCommand command in commands)
            {
                command.Run(info);
            }
        }

    }
}