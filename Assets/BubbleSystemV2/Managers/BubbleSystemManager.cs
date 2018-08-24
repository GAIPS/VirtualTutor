using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BubbleSystem2
{
    public class BubbleSystemManager : MonoBehaviour
    {
        public List<AbstractBubbleSystemModule> modules = new List<AbstractBubbleSystemModule>();

        private void Awake()
        {
            modules = GetComponents<AbstractBubbleSystemModule>().ToList();
        }

        public void UpdateScene(BubbleSystemData data)
        {
            foreach (AbstractBubbleSystemModule module in modules)
                module.UpdateScene(data);
        }
    }
}