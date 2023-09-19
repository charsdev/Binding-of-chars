using Chars.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chars.Pathfinding
{
    public class Dijkstra : PathFinding
    {
        public Dijkstra(Grid grid, Node start, Node end) : base(grid, start, end)
        {
        }

        public override List<Node> FindPath()
        {
            StartNode.TentativeCost = 0;
            Open.Clear();
            Close.Clear();

            foreach (var item in Grid.Nodes)
            {
                item.TentativeCost = UnityEngine.Random.Range(0, 10);
            }

            Open.Add(StartNode);

            while (Open.Count > 0)
            {
                CurrentNode = Open.First();
                CurrentNode = GetBestNode(CurrentNode);

                if (CurrentNode == EndNode)
                {
                    return GeneratePath();
                }

                Open.Remove(CurrentNode);
                Close.Add(CurrentNode);

                var adjs = Grid.GetAdjacentsNodes(CurrentNode, ref MathUtils.FourDirectionsInt);

                foreach (var adj in adjs)
                {
                    if (!Close.Contains(adj)
                        && Grid.Nodes[adj.GridPosition.x, adj.GridPosition.y].Type != (int)Tiles.OBSTACLE)
                    {
                        float tentativeCost = CurrentNode.TentativeCost + adj.TentativeCost;

                        if (adj.TentativeCost < CurrentNode.TentativeCost)
                        {
                            adj.Parent = CurrentNode;
                            adj.TentativeCost = tentativeCost;
                            Open.Add(adj);
                        }
                    }
                }
            }

            return new List<Node>();
        }

        private Node GetBestNode(Node currentNode)
        {
            return Open.FirstOrDefault(node => node.TentativeCost <= currentNode.TentativeCost);
        }

        private float GetManhattanDistance(Node source, Node target)
        {
            return Mathf.Abs(source.GridPosition.x - target.GridPosition.x) + Mathf.Abs(source.GridPosition.y - target.GridPosition.y);
        }
    }
}

