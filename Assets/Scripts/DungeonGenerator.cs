using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    public int cellWidth = 50;
    public int cellHeight = 44;
    public int screenW = 500;
    public int screenH = 330;

    public GameObject cellPrefab;
    public GameObject bossPrefab;
    public GameObject rewardPrefab;
    public GameObject coinPrefab;
    public GameObject secretPrefab;

    private bool started = false;
    private bool placedSpecial;
    [SerializeField] private GameObject[] images;
    private int[] floorplan;
    private int floorplanCount;
    private Queue<int> cellQueue;
    private List<int> endrooms;
    private int maxRooms = 15;
    private int minRooms = 7;
    private int bossIndex;

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (started)
        {
            if (cellQueue.Count > 0)
            {
                int i = cellQueue.Dequeue();
                int x = i % 10;
                bool created = false;
                if (x > 1) created |= Visit(i - 1);
                if (x < 9) created |= Visit(i + 1);
                if (i > 20) created |= Visit(i - 10);
                if (i < 70) created |= Visit(i + 10);
                if (!created)
                {
                    endrooms.Add(i);
                }
            }
            else if (!placedSpecial)
            {
                if (floorplanCount < minRooms)
                {
                    StartGame();
                    return;
                }

                placedSpecial = true;
                bossIndex = endrooms[endrooms.Count - 1];
                GameObject bossCell = CreateCell(bossPrefab, bossIndex);
                bossCell.transform.position += new Vector3(cellWidth, cellHeight, 0);

                int rewardIndex = PopRandomEndRoom();
                GameObject rewardCell = CreateCell(rewardPrefab, rewardIndex);

                int coinIndex = PopRandomEndRoom();
                CreateCell(coinPrefab, coinIndex);

                int secretIndex = PickSecretRoom();
                CreateCell(cellPrefab, secretIndex);
                CreateCell(secretPrefab, secretIndex);

                if (rewardIndex == -1 || coinIndex == -1 || secretIndex == -1)
                {
                    StartGame();
                    return;
                }
            }
        }
    }

    private void OnMouseDown()
    {
    }

    private void StartGame()
    {
        started = true;
        placedSpecial = false;
        DestroyImages();
        floorplan = new int[101];
        floorplanCount = 0;
        cellQueue = new Queue<int>();
        endrooms = new List<int>();
        Visit(45);
    }

    private bool Visit(int i)
    {
        if (floorplan[i] == 1)
            return false;

        int neighbours = NeighbourCount(i);

        if (neighbours > 1)
            return false;

        if (floorplanCount >= maxRooms)
            return false;

        if (Random.value < 0.5f && i != 45)
            return false;

        cellQueue.Enqueue(i);
        floorplan[i] = 1;
        floorplanCount += 1;

        CreateCell(cellPrefab, i);
        return true;
    }

    private int NeighbourCount(int i)
    {
        return floorplan[i - 10] + floorplan[i - 1] + floorplan[i + 1] + floorplan[i + 10];
    }

    private int PopRandomEndRoom()
    {
        if (endrooms.Count == 0)
            return -1;

        int index = Random.Range(0, endrooms.Count);
        int roomIndex = endrooms[index];
        endrooms.RemoveAt(index);
        return roomIndex;
    }

    private int PickSecretRoom()
    {
        for (int e = 0; e < 900; e++)
        {
            int x = Random.Range(1, 10);
            int y = Random.Range(2, 9);
            int i = y * 10 + x;

            if (floorplan[i] != 0)
                continue;

            if (bossIndex == i - 1 || bossIndex == i + 1 || bossIndex == i + 10 || bossIndex == i - 10)
                continue;

            if (NeighbourCount(i) >= 3)
                return i;

            if (e > 300 && NeighbourCount(i) >= 2)
                return i;

            if (e > 600 && NeighbourCount(i) >= 1)
                return i;
        }
        return -1;
    }

    private GameObject CreateCell(GameObject prefab, int index)
    {
        int x = index % 10;
        int y = (index - x) / 10;
        Vector3 position = new Vector3(screenW / 2 + cellWidth * (x - 5), screenH / 2 + cellHeight * (y - 4), 0);
        GameObject cell = Instantiate(prefab, position, Quaternion.identity);
        images[y * 10 + x] = cell;
        return cell;
    }

    private void DestroyImages()
    {
        if (images != null)
        {
            for (int i = 0; i < images.Length; i++)
            {
                if (images[i] != null)
                    Destroy(images[i]);
            }
        }
        images = new GameObject[100];
    }
}