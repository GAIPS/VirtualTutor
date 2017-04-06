using System;
using UnityEngine;

namespace VT
{
	[Serializable]
	public class Control :IControl
	{

		/* SUPPORT to create or destroy UI
		*/
		[SerializeField]

		public GameObject prefab;
		public GameObject instance;

		public Control ()
		{
		}

		public Control (GameObject prefab)
		{
			this.prefab = prefab;
		}

		public ShowResult Show ()
		{
			if (instance == null) {
				if (prefab == null)
					return ShowResult.FAIL;
				instance = (GameObject)GameObject.Instantiate (prefab, Vector3.zero, Quaternion.identity);
				if (!instance)
					return ShowResult.FAIL;
				GameObject.DontDestroyOnLoad (instance.gameObject);
				return ShowResult.FIRST;
			} else if (instance.activeSelf == false) {
				instance.SetActive (true);
			}
			return ShowResult.OK;
		}

		public void Destroy ()
		{
			if (instance == null)
				return;
			GameObject.Destroy (instance.gameObject);
			instance = null;
		}

		public void Disable ()
		{
			if (instance == null)
				return;
			instance.SetActive (false);
		}

		public bool IsVisible ()
		{
			return instance != null && instance.activeSelf;
		}
	}
}
