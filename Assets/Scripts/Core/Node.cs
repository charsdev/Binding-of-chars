using UnityEngine;

namespace Chars.Pathfinding
{
    public class Node
    {
        public int Type; // free cell
        public Vector2Int GridPosition;
        public Vector3 WorldPosition;
        public float Heuristic = 0; //h
        public float TentativeCost = 0; //g
        public float Priority = 0; //f 
        public bool Walkable = true;
        public Node Parent;
    }
}


