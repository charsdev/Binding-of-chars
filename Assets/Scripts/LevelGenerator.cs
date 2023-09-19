using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int _numberOfRooms = 20;
    [SerializeField] private Vector2Int _offset = new Vector2Int(16, 8);
    [SerializeField] private Room _roomPrefab;

    [SerializeField] private List<GameObject> _roomLayouts;

    [SerializeField] private GameObject _shopPrefab;
    public List<Room> Rooms { get; private set; }

    private readonly Vector2Int[] _directions = { Vector2Int.right, Vector2Int.down, Vector2Int.up, Vector2Int.left };

    private void Start()
    {
        Rooms = new List<Room>(_numberOfRooms);

        CreateRooms();
        SetDoors();

        Rooms[0].GetComponent<SpriteRenderer>().color = Color.green;
        Rooms[0].IsInitialRoom = true;
        Rooms[^1].GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void CreateRooms()
    {
        Rooms.Clear();

        Stack<Vector2Int> positions = new Stack<Vector2Int>();
        positions.Push(Vector2Int.zero);

        var initialRoom = CreateRoom(positions.Peek());
        initialRoom.IsInitialRoom = true;
        Rooms.Add(initialRoom);

        int iterator = 0;
        for (int i = 0; i < _numberOfRooms - 1; i++)
        {
            Vector2Int lastPosition = positions.Peek();

            if (!FoundValidPosition(lastPosition, positions, out Vector2Int nextPosition))
            {
                if (iterator > 100)
                {
                    foreach (var room in Rooms)
                    {
                        Destroy(room.gameObject);
                    }

                    CreateRooms();
                    return;
                }

                positions.Pop();
                i -= 2;
                iterator++;
                continue;
            }

            positions.Push(nextPosition);
            var newRoom = CreateRoom(nextPosition);
            Rooms.Add(newRoom);
        }

        const int maxShops = 3;
        int countShops = 0;
        int currentOffset = 5;
        int offset = 5;

        for (int i = 0; i < Rooms.Count; i++)
        {
            Room room = Rooms[i];
            if (!room.IsInitialRoom &&
                countShops < maxShops &&
                i == currentOffset)
            {
                room.RoomType = RoomType.SHOP;
                Instantiate(_shopPrefab, room.transform.position, Quaternion.identity, room.transform);
                countShops++;
                currentOffset += offset;
            }
            //else
            //{
            //    room.RoomType = RoomType.ENEMYROOM;
            //    room.GenerateObstacles();
            //}
        }
    }

    private void SetDoors()
    {
        for (int i = 0; i < Rooms.Count; i++)
        {
            var adjacentRooms = GetAdjacentRooms(Rooms[i]);

            for (int j = 0; j < adjacentRooms.Count; j++)
            {
                ConnectRooms(Rooms[i], adjacentRooms[j]);
            }

            HideUnconnectedDoors(Rooms[i]);
        }

        //for (int i = 0; i < Rooms.Count - 1; i++)
        //{
        //    Room currentRoom = Rooms[i];
        //    var adjacentRooms = GetAdjacentRooms(currentRoom);

        //    if (adjacentRooms.Count > 0)
        //    {
        //        var adjacentRoomsCount = Random.Range(1, adjacentRooms.Count);

        //        for (int j = 0; j < adjacentRoomsCount; j++)
        //        {
        //            Room adjacentRoom = adjacentRooms[j];
        //            //Door adjacentDoorTarget = adjacentRoom.GetAdjacentDoorTargetTo(currentRoom);
        //            //bool hasAdjacentDoor = adjacentDoorTarget != null;

        //            if (adjacentRoom != Rooms[^1])
        //            {
        //                CreateDoorAndConnectRooms(currentRoom, adjacentRoom);
        //            }
        //        }
        //    }
        //}

        //connect doors for the last room
        //var firstAdjacentRoom = GetAdjacentRooms(Rooms[^1])[0];
        //CreateDoorAndConnectRooms(Rooms[^1], firstAdjacentRoom);
    }

    private void HideUnconnectedDoors(Room room)
    {
        foreach (var door in room.Doors.GetDoors())
        {
            if (door.Target == null)
            {
                door.gameObject.SetActive(false);
            }
        }
    }

    private void ConnectRooms(Room room1, Room room2)
    {
        var deltaY = room2.transform.position.y - room1.transform.position.y;
        var deltaX = room2.transform.position.x - room1.transform.position.x;

        if (deltaY > 0)
        {
            room1.Doors.Updoor.SetTargetDoor(room2.Doors.DownDoor);
            room2.Doors.DownDoor.SetTargetDoor(room1.Doors.Updoor);
        }
        if (deltaY < 0)
        {
            room1.Doors.DownDoor.SetTargetDoor(room2.Doors.Updoor);
            room2.Doors.Updoor.SetTargetDoor(room1.Doors.DownDoor);
        }

        if (deltaX > 0)
        {
            room1.Doors.RightDoor.SetTargetDoor(room2.Doors.LeftDoor);
            room2.Doors.LeftDoor.SetTargetDoor(room1.Doors.RightDoor);
        }
        if (deltaX < 0)
        {
            room1.Doors.LeftDoor.SetTargetDoor(room2.Doors.RightDoor);
            room2.Doors.RightDoor.SetTargetDoor(room1.Doors.LeftDoor);
        }
    }

    private List<Room> GetAdjacentRooms(Room room)
    {
        List<Room> adjacentRooms = new List<Room>();

        foreach (var otherRoom in Rooms)
        {
            if (IsAdjacent(room.Position, otherRoom.Position))
            {
                adjacentRooms.Add(otherRoom);
            }
        }

        return adjacentRooms;
    }

    private bool FoundValidPosition(Vector2Int lastPosition, Stack<Vector2Int> positions, out Vector2Int nextPosition)
    {
        bool foundValidPosition = false;
        nextPosition = Vector2Int.zero;
        const int maxIteration = 50;

        for (int j = 0; j < maxIteration; j++)
        {
            nextPosition = GetRandomNextPosition(lastPosition);

            if (!positions.Contains(nextPosition))
            {
                foundValidPosition = true;
                break;
            }
        }

        return foundValidPosition;
    }

    private Room CreateRoom(Vector2Int currentPosition)
    {
        var room = Instantiate(_roomPrefab, (Vector2)currentPosition * _offset, Quaternion.identity, transform);
        room.Position = currentPosition;
        room.id = Rooms.Count();
        room.name = "Room " + room.id;
        return room;
    }

    private Vector2Int GetRandomNextPosition(Vector2Int currentPosition)
    {
        return currentPosition + _directions[Random.Range(0, _directions.Length)];
    }

    private bool IsAdjacent(Vector2Int position, Vector2Int nextPosition)
    {
        return Vector2Int.Distance(position, nextPosition) == 1;
    }
}