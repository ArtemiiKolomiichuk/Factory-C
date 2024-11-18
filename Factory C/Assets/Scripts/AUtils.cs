using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUtils
{
    public static int _playerID = 0;

    public static void InsertSorted<T>(List<T> list, T item) where T : IComparable<T>
    {
        int index = list.BinarySearch(item);
        if (index < 0)
        {
            index = ~index;
        }
        list.Insert(index, item);
    }

    public static void InsertSortedEnum<T>(List<T> list, T item) where T : Enum
    {
        var comparer = Comparer<T>.Default;
        int index = list.BinarySearch(item, comparer);

        if (index < 0)
        {
            index = ~index;
        }

        list.Insert(index, item);
    }

    public static float CalculateProximity(float value, float target, float rangeMin, float rangeMax)
    {
        value = Mathf.Clamp(value, rangeMin, rangeMax);

        float range = (rangeMax - rangeMin);
        float distanceToTarget = Mathf.Abs(value - target);
        float normalizedProximity = Mathf.Clamp01(1f - (distanceToTarget / range));

        return normalizedProximity;
    }


}
