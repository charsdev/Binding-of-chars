using Chars.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chars.Pathfinding
{
    public class AStar
    {
        private Grid Grid;

        private HashSet<Node> Open = new HashSet<Node>();
        private HashSet<Node> Close = new HashSet<Node>();
        private List<Node> Path = new List<Node>();

        public AStar(Grid grid)
        {
            Grid = grid;
        }

        public void SetGrid(Grid grid)
        {
            Grid = grid;
        }

        public List<Node> FindPath(Node start, Node end)
        {
            Open.Clear();
            Close.Clear();
            Path.Clear();

            Open.Add(start);

            while (Open.Count > 0)
            {
                var currentNode = Open.First();

                foreach (var node in Open)
                {
                    if (node.Priority <= currentNode.Priority)
                    {
                        currentNode = node;
                    }
                }

                if (currentNode == end)
                {
                    return GeneratePath(start, end);
                }

                Open.Remove(currentNode);
                Close.Add(currentNode);

                foreach (var adj in Grid.GetAdjacentsNodes(currentNode, ref MathUtils.EightDirectionsInt))
                {
                    if (!Close.Contains(adj)
                        && Grid.Nodes[adj.GridPosition.x, adj.GridPosition.y].Type != (byte)Tiles.OBSTACLE)
                    {
                        var tentativeCost = adj.TentativeCost + GetManhattanDistance(currentNode, adj);

                        if (tentativeCost < adj.TentativeCost || !Open.Contains(adj))
                        {
                            adj.Parent = currentNode;
                            adj.TentativeCost = tentativeCost;
                            adj.Heuristic = GetManhattanDistance(adj, end);
                            adj.Priority = adj.TentativeCost + adj.Heuristic;

                            Open.Add(adj);
                        }
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

        private float GetManhattanDistance(Node source, Node target)
        {
            if (source != null && target != null)
            {
                var a = Mathf.Abs(source.GridPosition.x - target.GridPosition.x);
                var b = Mathf.Abs(source.GridPosition.y - target.GridPosition.y);

                if (a > b)
                {
                    return (Grid.Width * b) + (Grid.Height * (a - b));
                }
                else
                {
                    return (Grid.Width * a) + (Grid.Height * (b - a));
                }
            }

            return 0;
        }
    }
}
