using System.Collections.Generic;
using UnityEngine;
using MathUtils = Chars.Utils.MathUtils;
using System.Linq;

namespace Chars.Pathfinding
{
    [System.Serializable]
    public class Grid
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Node[,] Nodes { get; set; }
        public Vector3 Center { get; private set; }

        private List<Node> _neighbors = new();

        public readonly float CellSize;
        public readonly int HalfHeight;
        public readonly int HalfWidth;
        public readonly int WidthMinusOne;
        public readonly int HeightMinusOne;

        public LayerMask obstacleLayer;

        public Grid(int width, int height, Vector3 center, float cellSize)
        {
            Width = width;
            Height = height;
            Nodes = new Node[width, height];
            CellSize = cellSize;
            HalfHeight = Height >> 1;
            HalfWidth = Width >> 1;
            HeightMinusOne = Height - 1;
            WidthMinusOne = Width - 1;
            Center = center;

            Vector2 bottonLeft = Center - Vector3.right * HalfWidth - Vector3.up * HalfHeight;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var point = bottonLeft + new Vector2(x, y) * cellSize;

                    Nodes[x, y] = new Node
                    {
                        Type = (int)Tiles.FREE,
                        GridPosition = new Vector2Int(x, y),
                        TentativeCost = Random.Range(0, 10),
                        WorldPosition = point
                    };
                }
            }
        }

        public Node FindNodeByWorldPosition(Vector3 worldPosition)
        {
            var localPosition = worldPosition - Center;
            int x = Mathf.RoundToInt((localPosition.x / CellSize) + HalfWidth + 1);
            int y = Mathf.RoundToInt((localPosition.y / CellSize) + HalfHeight + 1);

            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return Nodes[x, y];
            }

            return null;
        }

        public Node GetRandomNode()
        {
            int randomX = Random.Range(0, Width);
            int randomY = Random.Range(0, Height);

            return Nodes[randomX, randomY];
        }

        public bool IsNodeInOpenRegion(Node node)
        {
            return (GetAdjacentsNodes(node, ref MathUtils.FourDirectionsInt).Where(adj => adj.Type is (int)Tiles.FREE)).Count() > 0;
        }

        public List<Node> GetAdjacentsNodes(Node node, ref Vector2Int[] directions)
        {
            _neighbors.Clear();

            foreach (var direction in directions)
            {
                Vector2Int neighborPos = node.GridPosition + direction;

                if (MathUtils.InsideGridLimits(neighborPos.x, neighborPos.y, Width, Height))
                {
                    var neighborNode = Nodes[neighborPos.x, neighborPos.y];
                    _neighbors.Add(neighborNode);
                }
            }

            return _neighbors;
        }
    }
}


