﻿using System.Collections;
using System.Collections.Generic;

public static class Helper
{
    public static T[] Shuffle<T>(T[] array, int seed)
    {
        System.Random rng = new System.Random(seed);

        for (int i = 0; i < array.Length - 1; i++)
        {
            int randomIndex = rng.Next(i, array.Length);
            T element = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = element;
        }

        return array;
    }
}