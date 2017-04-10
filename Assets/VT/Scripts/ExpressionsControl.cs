using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace VT
{
	[Serializable]
	public class ExpressionsControl : IControl
	{
		private Control control;
		private Topic currentTopic;
		private int lineOffset = 0;
		public int LineOffset {
			get {
				return this.lineOffset;
			}
			set {
				lineOffset = value;
			}
		}

		public ExpressionsControl (GameObject prefab)
		{
			control = new Control ();
			control.prefab = prefab;
		}

		public void Set (Topic currentTopic)
		{
			lineOffset = 0;
			this.currentTopic = currentTopic;
		}

		public ShowResult Show ()
		{
			var ret = control.Show ();
			if (ret == ShowResult.FIRST || ret == ShowResult.OK) {
				var hooks = control.instance.GetComponent<ExpressionsHooks> ();
				if (hooks) {
					if (hooks.LeftSprites == null)
						hooks.LeftSprites = new List<Sprite> ();
					if (hooks.RightSprites == null)
						hooks.RightSprites = new List<Sprite> ();
					if (currentTopic.Lines [lineOffset].Speaker.IsLeft) {
						hooks.LeftContent = currentTopic.Lines [LineOffset].Content;
						hooks.RightContent = currentTopic.Lines [lineOffset+1].Content;
						hooks.LeftSprite = hooks.LeftSprites [(int)currentTopic.Lines[lineOffset].Speaker.CurrentEmotion];
						hooks.RightSprite = hooks.RightSprites [(int)currentTopic.Lines[lineOffset+1].Speaker.CurrentEmotion];
					} else if (!currentTopic.Lines [lineOffset].Speaker.IsLeft) {
						hooks.RightContent = currentTopic.Lines [lineOffset].Content;
						hooks.LeftContent = currentTopic.Lines [lineOffset+1].Content;
						hooks.RightSprite = hooks.RightSprites [(int)currentTopic.Lines [lineOffset].Speaker.CurrentEmotion];
						hooks.LeftSprite = hooks.LeftSprites [(int)currentTopic.Lines [lineOffset+1].Speaker.CurrentEmotion];
					}
				}
			}
			return ret;
			
		}
		public void UpdateControl(){
			lineOffset += 2;
			Show ();
		}

		public ShowResult SetAndShow (Topic currentTopic)
		{
			this.Set (currentTopic);
			return Show ();
		}

		public void Destroy ()
		{
			control.Destroy ();
		}

		public void Disable ()
		{
			control.Disable ();
		}

		public bool IsVisible ()
		{
			return control.IsVisible ();
		}

	}
}
