﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ReflectiveEnumerator
{
    static ReflectiveEnumerator() { }

    public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class
    {
        List<T> objects = new List<T>();
        IEnumerable a = Assembly.GetAssembly(typeof(T)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)));

        foreach (Type type in a)
            objects.Add((T)Activator.CreateInstance(type, constructorArgs));

        return objects;
    }
}