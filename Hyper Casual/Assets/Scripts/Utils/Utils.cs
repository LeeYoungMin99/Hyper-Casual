using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static Collider[] Colliders = new Collider[16];
    public static readonly Vector2 ZERO_VECTOR2 = Vector2.zero;
    public static readonly Vector3 ZERO_VECTOR3 = Vector3.zero;
    public static readonly Quaternion IDEN_QUAT = Quaternion.identity;

    public static float CalculateAngle(Vector3 targetDir, Vector3 myForward)
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
    public const int MAP_LAYER = 3;
    public const int WALL_LAYER = 6;
    public const int FRIENDLY_PROJECTILE = 7;
    public const int ENEMY_PROJECTILE = 8;
    public const int WALL_LAYER_MASK = 1 << 6;
    public const int WALL_AND_MAP_LAYER_MASK = (1 << 6) + (1 << 3);
    public const int ALL_ENEMY_LAYER_MASK = (1 << 13) + (1 << 14) + (1 << 15) + (1 << 16);
    public const int ALL_PLAYER_LAYER_MASK = (1 << 9) + (1 << 10) + (1 << 11) + (1 << 12);

    public static readonly int[] PLAYER_LAYERS = new int[4] { 9, 10, 11, 12 };
}
