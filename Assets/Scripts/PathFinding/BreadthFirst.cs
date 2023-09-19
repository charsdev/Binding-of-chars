using Chars.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Chars.Pathfinding
{
    public class BreadthFirst
    {
        HashSet<Node> Open = new HashSet<Node>();
        HashSet<Node> Close = new HashSet<Node>();
        List<Node> Path = new List<Node>();

        public List<Node> FindPath(Node start, Node end, ref Grid grid)
        {
            Path.Clear();
            Open.Clear();
            Close.Clear();

            Open.Add(start);
            Close.Add(start);

            while (Open.Count > 0)
            {
                Node currentNode = Open.First();
                Open.Remove(currentNode);

                if (currentNode == end)
                {
                    return GeneratePath(start, end);
                }

                foreach (var neighbor in grid.GetAdjacentsNodes(currentNode, ref MathUtils.FourDirectionsInt))
                {
                    if (!Close.Contains(neighbor)
                        && !IsObstacle(neighbor, ref grid))
                    {
                        Open.Add(neighbor);
                        Close.Add(neighbor);
                        neighbor.Parent = currentNode;
                    }
                }
            }

            return new List<Node>();
        }


        protected List<Node> GeneratePath(Node startNode, Node targetNode)
        {
            Node currentNode = targetNode;

            while (currentNode != startNode)
            {
                Path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            Path.Reverse();
            return Path;
        }

        public bool IsObstacle(Node node, ref Grid grid)
        {
            return grid.Nodes[node.GridPosition.x, node.GridPosition.y].Type == (byte)Tiles.OBSTACLE;
        }
    }
}
