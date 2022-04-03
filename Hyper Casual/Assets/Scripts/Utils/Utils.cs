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
