using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class Comparison
{
    public enum Type
    {
        Less,
        LessOrEqual,
        Equal,
        Greater,
        GreaterOrEqual,
    }

    public static bool Compare(float left, float right, Type type)
    {
        switch (type)
        {
            case Type.Less:
                return left < right;
            case Type.LessOrEqual:
                return left <= right;
            case Type.Equal:
                return left == right;
            case Type.Greater:
                return left > right;
            case Type.GreaterOrEqual:
                return left >= right;
            default:
                break;
        }
        return false;
    }
}
