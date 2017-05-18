using System;
using UnityEngine;

namespace VT
{
	public delegate void IntFunc(int value);

	public delegate void BoolFunc(bool value);

	public delegate void StringFunc(string value);

	public delegate void VoidFunc();

	public delegate void GameObjectFunc(GameObject gameObj);
	public delegate void FloatFunc(float value);
}

