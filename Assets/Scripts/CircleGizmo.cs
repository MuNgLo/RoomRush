using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public static class GizmoExtension
{
    static Vector3 top = Vector3.zero;

    public static void DrawGizmosCone(Vector3 pos, float angle, float height, Vector3 up, Color color, bool inverse = false, int step = 10)
    {
        float radius = height * Mathf.Tan(angle * Mathf.Deg2Rad * 0.5f);

        if (inverse)
        {
            top = pos;
            DrawGizmosCircle(pos + up * height, radius, up, color, step, (v) => { Gizmos.DrawLine(top, v); });
        }
        else
        {
            top = pos + up * height;
            DrawGizmosCircle(pos, radius, up, color, step, (v) => { Gizmos.DrawLine(top, v); });
        }
    }

    public static void DrawGizmosCircle(Vector3 pos, float radius, Vector3 up, Color color, int step = 10, Action<Vector3> action = null)
    {
        float theta = 360f / (float)step;
        Vector3 cross = Vector3.Cross(up, Vector3.up);
        if (cross.magnitude == 0f)
        {
            cross = Vector3.forward;
        }
        Vector3 prev = pos + Quaternion.AngleAxis(0f, up) * cross * radius;
        Vector3 next = prev;
        Gizmos.color = color;
        for (int i = 1; i <= step; ++i)
        {
            next = pos + Quaternion.AngleAxis(theta * (float)i, up) * cross * radius;
            Gizmos.DrawLine(prev, next);
            if (null != action)
            {
                action(prev);
            }
            prev = next;
        }
    }

    



}//EOF CLASS