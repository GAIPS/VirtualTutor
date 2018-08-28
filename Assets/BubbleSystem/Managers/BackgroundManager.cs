using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem
{
    public class BackgroundManager : AbstractBubbleSystemModule
    {
        public Background[] backgrounds;

        public override void UpdateScene(BubbleSystemData data)
        {
            if (data.backgroundData.IsCleared()) return;
            foreach (Background bg in backgrounds)
                bg.UpdateScene(data);
        }
    }
}