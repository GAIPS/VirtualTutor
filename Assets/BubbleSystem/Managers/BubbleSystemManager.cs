using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BubbleSystem
{
    public class BubbleSystemManager : AbstractBubbleSystemModule
    {
        public List<AbstractBubbleSystemModule> modules = new List<AbstractBubbleSystemModule>();

        private void Awake()
        {
            modules = GetComponents<AbstractBubbleSystemModule>().ToList();
            modules.Remove(this);
        }

        public override void UpdateScene(BubbleSystemData data)
        {
            foreach (AbstractBubbleSystemModule module in modules)
                module.UpdateScene(data);
        }
    }
}