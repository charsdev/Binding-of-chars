using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private LevelGenerator _levelGenerator;
    public Room CurrentRoom;
    public static UnityEvent<Room> OnRoomChanged = new UnityEvent<Room>();
    public GameObject EnemyPrefab;

    private void Start()
    {
        if (_levelGenerator != null)
        {
            CurrentRoom = _levelGenerator.Rooms.First();
        }

        if (CurrentRoom != null)
        {
            CurrentRoom.BlockDoors();
        }

        OnRoomChanged.AddListener((room) =>
        {
            Debug.Log("Room");
            CurrentRoom = room;
        });
    }

    private void Update()
    {
        if (CurrentRoom != null)
        {
            if (CurrentRoom.Enemies.All(x => x == null))
            {
                CurrentRoom.UnBlockDoors();
            }
        }
    }
    
    public void GenerateEnemy()
    {
        print("Generate Enemy");
        int randomIndex = Random.Range(0, CurrentRoom.EnemyPositions.Length);
        Instantiate(EnemyPrefab, CurrentRoom.EnemyPositions[randomIndex].position, Quaternion.identity);
    }
}
