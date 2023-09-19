using BindingOfChars;
using System.Collections.Generic;
using UnityEngine;
using Chars.Pathfinding;

[System.Serializable]
public struct DoorWrapper
{
    public Door Updoor;
    public Door LeftDoor;
    public Door RightDoor;
    public Door DownDoor;

    public List<Door> GetDoors()
    {
        return new List<Door> { Updoor, RightDoor, LeftDoor, DownDoor };
    }
}

public enum RoomType
{
    INITIAL,
    ENEMYROOM,
    BOSS,
    SHOP
}

public class Room : MonoBehaviour
{
    public DoorWrapper Doors = new DoorWrapper();
    public List<EnemyController> Enemies = new List<EnemyController>();
    
    [SerializeField] private Transform[] _positions = new Transform[4];

    public bool IsBlocked;
    public bool IsInitialRoom;
    public Vector2Int Position;
    public int id = 0;

    public RoomType RoomType;

    [SerializeField] private GameObject _instructions;
    [SerializeField] private EnemyGenerator _enemyGenerator;
    [SerializeField] private GameObject _coin;
    [SerializeField] private bool _visited = false;

    public bool GenerateEnemies;
    public Collider2D Walls;
    public ObstacleGenerator ObstacleGenerator;
    public GridController GridController;
    public Transform[] EnemyPositions { get => _positions;}

    public bool IsInitialized;

    public void Start()
    {
        if (IsInitialRoom)
        {
            ShowInstructions();
        }

        //if (!IsInitialRoom && GenerateEnemies)
        //{
        //    if (GridController.Grid != null)
        //    {
        //        GenerateObstacles();

        //        Enemies.AddRange(_enemyGenerator.GenerateEnemies(GridController));

        //        foreach (EnemyController enemy in Enemies)
        //        {
        //            int randomPositionIndex = Random.Range(0, EnemyPositions.Length);
        //            enemy.transform.position = EnemyPositions[randomPositionIndex].position;
        //        }
        //    }
        //}
    }

    public void EnterRoom()
    {
        if (Enemies.Count > 0)
        {
            EnableEnemies();
            BlockDoors();
        }

        // we can split this after in a new class as a command for example. Then call this command class
        // here for better readeability like a component
        
        // if not was visited and we have a grid here, use the lucky stat to spawn the coin randomly in a free space
        if (!_visited && GridController.Grid != null && GridController.Grid.Nodes != null)
        {
            foreach (var cell in GridController.Grid.Nodes)
            {
                if (cell.Type == (int)Tiles.FREE && Random.Range(0f, 1f) > GameManager.Instance.PlayerStats.Lucky)
                {
                    //we need to use a singleton because we dont use resources call...
                    Instantiate(Prefabs.Coin, cell.WorldPosition, Quaternion.identity);
                }
            }
        }

        _visited = true;
    }

    //public void GenerateObstacles()
    //{
    //    if (!IsInitialized && !IsInitialRoom)
    //    {
    //        var currentDoors = new List<Door>();
    //        foreach (var item in Doors.GetDoors())
    //        {
    //            if (item.gameObject.activeInHierarchy)
    //            {
    //                currentDoors.Add(item);
    //            }
    //        }

    //        ObstacleGenerator.Initialize(currentDoors, id);
    //        IsInitialized = true;
    //    }
    //}

    private void EnableEnemies()
    {
        foreach (EnemyController enemy in Enemies)
        {
            if (enemy != null)
            {
                enemy.gameObject.SetActive(true);
            }
        }
    }

    public void BlockDoors()
    {
        IsBlocked = true;

        foreach (Door door in Doors.GetDoors())
        {
            door?.Block();
        }
    }
    public void UnBlockDoors()
    {
        IsBlocked = false;

        foreach (Door door in Doors.GetDoors())
        {
            door?.UnBlock();
        }
    }

    private void ShowInstructions()
    {
        if (_instructions != null)
        {
            _instructions.SetActive(true);
        }
    }

    public Door GetAdjacentDoorTargetTo(Room targetRoom)
    {
        Door[] doorList = GetComponentsInChildren<Door>();

        foreach (var door in doorList)
        {
            if (door.Target != null && door.Target.transform.parent == targetRoom.transform)
            {
                return door;
            }
        }

        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EnterRoom();
            RoomManager.OnRoomChanged.Invoke(this);
        }
    }
}
