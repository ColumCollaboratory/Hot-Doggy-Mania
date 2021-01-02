using System;
using UnityEngine;

public static class DebugUtilities
{
    public static void HatchBox2D(Vector2 cornerA, Vector2 cornerB, float hatchStep)
    {
        Vector2 min = new Vector2
        {
            x = Mathf.Min(cornerA.x, cornerB.x),
            y = Mathf.Min(cornerA.y, cornerB.y)
        };
        Vector2 max = new Vector2
        {
            x = Mathf.Max(cornerA.x, cornerB.x),
            y = Mathf.Max(cornerA.y, cornerB.y)
        };

        float sizeX = max.x - min.x;
        float sizeY = max.y - min.y;

        float distance = hatchStep;

        while (distance <= sizeX * 2f || distance <= sizeY * 2f)
        {
            Vector2 start = min + Vector2.up * distance;
            if (distance > sizeY)
                start += new Vector2(distance - sizeY, sizeY - distance);
            Vector2 end = min + Vector2.right * distance;
            if (distance > sizeX)
                end += new Vector2(sizeX - distance, distance - sizeX);
            Gizmos.DrawLine(start, end);
            distance += hatchStep;
        }

        Gizmos.DrawLine(min, min + Vector2.up * sizeY);
        Gizmos.DrawLine(min, min + Vector2.right * sizeX);
        Gizmos.DrawLine(max, max - Vector2.up * sizeY);
        Gizmos.DrawLine(max, max - Vector2.right * sizeX);
    }
}