using UnityEditor;
using UnityEngine;

namespace Chars.Pathfinding
{
    public class GridController : MonoBehaviour
    {
        public int Width;
        public int Height;
        public Grid Grid;
        [SerializeField] private float _cellSize = 1.25f;

        private void Awake()
        {
            Grid = new Grid(Width, Height, transform.position, _cellSize);
        }

        private void DrawGrid(ref Grid grid)
        {
            var floorHalfWidth = Width >> 1;
            var floorHalfHeight = Height >> 1;

            Vector2 bottonLeft = transform.position - Vector3.right * floorHalfWidth - Vector3.up * floorHalfHeight;
            var WorldPosition = bottonLeft + new Vector2(floorHalfWidth, floorHalfHeight) * _cellSize;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(WorldPosition, new Vector3(Width * _cellSize, Height * _cellSize));

            if (!Application.isPlaying)
                return;

            if (grid == null) return;
            if (grid.Nodes == null) return;

            foreach (var node in grid.Nodes)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(node.WorldPosition, Vector3.one * (_cellSize - 0.1f));
                //GUIStyle style = new GUIStyle();
                //style.normal.textColor = Color.black;
                //Handles.Label(node.WorldPosition, $"{node.GridPosition.x},{node.GridPosition.y}", style);
            }
        }

        private void OnDrawGizmos()
        {
            DrawGrid(ref Grid);
        }
    }
}