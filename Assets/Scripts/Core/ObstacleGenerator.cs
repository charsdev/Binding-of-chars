using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Chars.Pathfinding
{
    public class ObstacleGenerator : MonoBehaviour
    {
        public int Width;
        public int Height;
        [SerializeField] private GridController _gridController;

        [SerializeField] private float _cellSize = 1.25f;
        [SerializeField] private LayerMask _obstacleLayer;
        [SerializeField] private LayerMask _doorLayer;
        [SerializeField] private GameObject _obstaclePrefab;
        private bool _initialized = false;

        public void Initialize(List<Door> doors, int Id)
        {
            _initialized = true;
            List<Vector2Int> doorPositions = new List<Vector2Int>();

            foreach (var door in doors)
            {
                var node = _gridController.Grid.FindNodeByWorldPosition(door.BoxCollider.bounds.center);

                if (node != null)
                {
                    node.Type = (int)Tiles.DOOR;
                    doorPositions.Add(node.GridPosition);
                }
            }

            GenerateValidLayout(doorPositions);
            PutObstacleTiles();
        }

        private void PutObstacleTiles()
        {
            foreach (var node in _gridController.Grid.Nodes)
            {
                if (node.Type == (int)Tiles.OBSTACLE)
                {
                    Instantiate(_obstaclePrefab, node.WorldPosition, Quaternion.identity);
                }
            }
        }

        private void LateUpdate()
        {
            CheckBoard();
        }

        private void CheckBoard()
        {
            if (_gridController == null)
                return;

            foreach (var node in _gridController.Grid.Nodes)
            {
                if (node.Type == (int)Tiles.DOOR)
                {
                    continue;
                }

                bool obstacleCollision = Physics2D.OverlapBox(node.WorldPosition, Vector2.one, 0, _obstacleLayer);

                //if the node walkable and the type is free and have a collision then set as obstacle
                if (node.Walkable && node.Type == (int)Tiles.FREE && obstacleCollision)
                {
                    node.Walkable = false;
                    node.Type = (int)Tiles.OBSTACLE;
                }
                else if (!node.Walkable && node.Type == (int)Tiles.OBSTACLE && !obstacleCollision)
                {
                    node.Walkable = true;
                    node.Type = (int)Tiles.FREE;
                }
            }
        }

        private void GenerateValidLayout(List<Vector2Int> doorPositions)
        {
            bool allDoorsHasPath = false;
            var pathfinding = new BreadthFirst();

            do
            {

                allDoorsHasPath = true;
                ClearGrid();
                GenerateObstacles();

                if (doorPositions.Count == 1)
                {
                    doorPositions.Add(new Vector2Int(_gridController.Grid.HalfWidth, _gridController.Grid.HalfHeight));
                }

                for (int i = 0; i < doorPositions.Count - 1; i++)
                {
                    var start = _gridController.Grid.Nodes[doorPositions[i].x, doorPositions[i].y];
                    var end = _gridController.Grid.Nodes[doorPositions[i + 1].x, doorPositions[i + 1].y];

                    var currentPath = pathfinding.FindPath(start, end, ref _gridController.Grid);

                    if (currentPath.Count == 0)
                    {
                        allDoorsHasPath = false;
                        break; // Exit the loop if a valid path is not found
                    }
                }
            }
            while (!allDoorsHasPath);

            if (!allDoorsHasPath)
            {
                invalidRoom = true;
            }
        }

        bool invalidRoom = false;

        private void GenerateObstacles()
        {
            int obstacleCount = 8; //Random.Range(0, 8);
            int randomIndexRegion = Random.Range(0, 2);
            bool mirror = Random.Range(0f, 1f) > 0.5f;

            Vector2Int[] regions = new Vector2Int[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(0, _gridController.Grid.HalfHeight)
            };

            GenerateObstaclesAtRegion(_gridController.Grid.HalfHeight, _gridController.Grid.HalfWidth, regions[randomIndexRegion], ref obstacleCount);
            InvertRegions(_gridController.Grid.HalfHeight, _gridController.Grid.HalfWidth, randomIndexRegion);

            if (mirror)
            {
                MirrorGrid();
            }
        }

        private void InvertRegions(int halfHeight, int halfWidth, int randomIndexRegion)
        {
            bool allowSimetry = Random.Range(0f, 1f) > 0.5f;

            for (int y = 0; y <= halfHeight; y++)
            {
                for (int x = 0; x <= halfWidth; x++)
                {
                    if (randomIndexRegion == 0)
                    {
                        //invierte lado 1 en lado 4;
                        if (IsFreeTile(x + halfWidth, y + halfHeight) && !IsDoorTile(halfWidth - x, halfHeight - y))
                        {
                            _gridController.Grid.Nodes[x + halfWidth, y + halfHeight].Type = _gridController.Grid.Nodes[halfWidth - x, halfHeight - y].Type;
                        }

                        if (!allowSimetry)
                            continue;

                        //invierte lado 4 en lado 3;
                        if (IsFreeTile(x + halfWidth, y) && !IsDoorTile(x, halfHeight - y))
                        {
                            _gridController.Grid.Nodes[x, y + halfHeight].Type = _gridController.Grid.Nodes[x, halfHeight - y].Type;
                        }

                        //invierte lado 3 en lado 2;
                        if (IsFreeTile(x + halfWidth, y) && !IsDoorTile(halfWidth - x, y))
                        {
                            _gridController.Grid.Nodes[x + halfWidth, y].Type = _gridController.Grid.Nodes[halfWidth - x, y].Type;
                        }
                    }
                    else
                    {
                        //invierte lado 3 en lado 2;
                        if (IsFreeTile(x + halfWidth, y) && !IsDoorTile(halfWidth - x, halfWidth - y))
                        {
                            _gridController.Grid.Nodes[x + halfWidth, y].Type = _gridController.Grid.Nodes[halfWidth - x, halfWidth - y].Type;
                        }

                        if (!allowSimetry)
                            continue;

                        //invierte lado 2 en lado 4;
                        if (IsFreeTile(x + halfWidth, y + halfHeight) && !IsDoorTile(halfWidth - x, halfHeight + y))
                        {
                            _gridController.Grid.Nodes[x + halfWidth, y + halfHeight].Type = _gridController.Grid.Nodes[halfWidth - x, halfHeight + y].Type;
                        }

                        //invierte lado 4 en lado 1;
                        if (IsFreeTile(halfWidth - x, halfHeight - y) && !IsDoorTile(halfWidth - x, halfHeight + y))
                        {
                            _gridController.Grid.Nodes[halfWidth - x, halfHeight - y].Type = _gridController.Grid.Nodes[halfWidth - x, halfHeight + y].Type;
                        }
                    }
                }
            }
        }

        private void MirrorGrid()
        {
            Node[,] mirrorGrid = new Node[_gridController.Grid.Width, _gridController.Grid.Height];

            for (int y = 0; y < _gridController.Grid.Height; y++)
            {
                for (int x = 0; x < _gridController.Grid.Width; x++)
                {
                    mirrorGrid[x, y] = _gridController.Grid.Nodes[_gridController.Grid.WidthMinusOne - x, y];
                }
            }

            _gridController.Grid.Nodes = mirrorGrid;
        }

        private void GenerateObstaclesAtRegion(int halfHeight, int halfWidth, Vector2Int region, ref int obstacleCount)
        {
            for (int y = 0; y <= halfHeight; y++)
            {
                for (int x = 0; x <= halfWidth; x++)
                {
                    if (Random.Range(0f, 1f) > 0.5f && obstacleCount > 0)
                    {
                        var currentCellPosition = new Vector2Int(x + region.x, y + region.y);

                        if (IsFreeTile(currentCellPosition.x, currentCellPosition.y))
                        {
                            _gridController.Grid.Nodes[currentCellPosition.x, currentCellPosition.y].Type = (int)Tiles.OBSTACLE;
                            obstacleCount--;
                        }
                    }
                }
            }
        }

        private bool IsFreeTile(int x, int y)
        {
            return _gridController.Grid.Nodes[x, y].Type == (int)Tiles.FREE;
        }

        private bool IsDoorTile(int x, int y)
        {
            return _gridController.Grid.Nodes[x, y].Type == (int)Tiles.DOOR;
        }

        private void ClearGrid()
        {
            if (_gridController.Grid.Nodes != null)
            {
                foreach (var node in _gridController.Grid.Nodes)
                {
                    if (node.Type != (int)Tiles.FREE && node.Type != (int)Tiles.DOOR)
                    {
                        node.Type = (int)Tiles.FREE;
                    }
                }
            }
        }
       
    }
}