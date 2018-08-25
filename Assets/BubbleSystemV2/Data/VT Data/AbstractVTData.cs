﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleSystem2
{
    public abstract class AbstractVTData : AbstractBubbleSystemData
    {
        private string _name = "";

        protected T Get<T>()
        {
            object success;
            if (EnumUtils.TryParse(typeof(T), _name, out success))
            {
                return (T)success;
            }
            throw new NullReferenceException("Tutor is not set.");
        }

        public abstract string GetString();

        protected void Set<T>(string value)
        {
            object success;
            Clear();
            if (EnumUtils.TryParse(typeof(T), value, out success))
            {
                _name = success.ToString();
            }
        }

        public override bool IsCleared()
        {
            return String.IsNullOrEmpty(_name);
        }

        public override void Clear()
        {
            _name = "";
        }
    }
}