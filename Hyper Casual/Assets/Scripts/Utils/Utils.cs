using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static readonly Utils Instance = new Utils();

    public float CalculateAngle(Vector3 targetDir, Vector3 myForward)
    {
        float dot = Mathf.Clamp(Vector3.Dot(targetDir, myForward), -1f, 1f);

        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        Vector3 cross = Vector3.Cross(myForward, targetDir);

        if (0f > cross.y)
        {
            angle *= -1;
        }

        return angle;
    }
}

public class LayerValue
{
    public const int MAP_OBJECT_LAYER = 6;
    public const int MAP_OBJECT_LAYER_MASK = 1 << 6;
    public const int ALL_ENEMY_LAYER_MASK = (1 << 13) + (1 << 14) + (1 << 15) + (1 << 16);
}
