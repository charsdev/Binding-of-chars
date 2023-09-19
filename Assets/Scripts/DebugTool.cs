using UnityEngine;

namespace Chars.Tools
{
    public class DebugTool
    {
        public static RaycastHit2D RaycastHit(Vector2 origin, Vector2 direction, float distance, LayerMask layerMask, Color color, bool drawGizmo = false)
        {
            if (drawGizmo)
                Debug.DrawRay(origin, direction * distance, color);
            
            return Physics2D.Raycast(origin, direction, distance, layerMask);
        }

        public static RaycastHit2D RaycastHitNonAlloc(Vector2 origin, Vector2 direction, float distance, LayerMask layerMask, Color color, bool drawGizmo = false)
        {
            if (drawGizmo)
                Debug.DrawRay(origin, direction * distance, color);

            return Physics2D.Raycast(origin, direction, distance, layerMask);
        }

        public static RaycastHit2D BoxCastHit(Vector2 origin, Vector2 size, Vector2 direction, float distance, LayerMask layerMask, Color color, bool drawGizmo = false)
        {
            return Physics2D.BoxCast(origin, size, 0, direction, distance, layerMask);
        }

    }
}
