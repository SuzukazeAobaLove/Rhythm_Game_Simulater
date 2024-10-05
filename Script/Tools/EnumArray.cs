using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DEnumArray<TEnum1, TEnum2, TValue>
where TEnum1 : Enum
where TEnum2 : Enum
{
    private TValue[] array;

    public DEnumArray()
    {
        int size1 = Enum.GetValues(typeof(TEnum1)).Length;
        int size2 = Enum.GetValues(typeof(TEnum2)).Length;
        array = new TValue[size1 * size2];
    }

    public TValue this[TEnum1 enum1, TEnum2 enum2]
    {
        get { return array[CalculateIndex(enum1, enum2)]; }
        set { array[CalculateIndex(enum1, enum2)] = value; }
    }

    private int CalculateIndex(TEnum1 enum1, TEnum2 enum2)
    {
        int index1 = Convert.ToInt32(enum1);
        int index2 = Convert.ToInt32(enum2);
        int size2 = Enum.GetValues(typeof(TEnum2)).Length;
        return index1 * size2 + index2;
    }
}

