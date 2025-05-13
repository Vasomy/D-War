using System;
using UnityEngine;


public class EnumOperator
{
    public static bool HasEnum<T>(T enum_target,T bit) where T : Enum
    {
        return ( GetIntFromEnum(enum_target) & GetIntFromEnum(bit) ) != 0;
    }

    static int GetIntFromEnum<T>(T target)where T : Enum
    {
        return (int)Enum.ToObject(typeof(T), target);
    }
}

