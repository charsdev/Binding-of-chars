using BindingOfChars;
using Chars.Pathfinding;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private EnemyController _enemyPrefab;
    [SerializeField] private int _maxEnemies;

    public List<EnemyController> GenerateEnemies()
    {
        var randomCount = Random.Range(1, 1);
        List<EnemyController> enemies = new List<EnemyController>();

        for (int i = 0; i < randomCount; i++)
        {
            var enemy = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
            enemy.gameObject.SetActive(false);
            enemies.Add(enemy);
        }

        return enemies;
    }

    public List<EnemyController> GenerateEnemies(GridController gridController)
    {
        var randomCount = Random.Range(1, 3);
        var enemies = new List<EnemyController>();
        Node node = null;

        for (int i = 0; i < randomCount; i++)
        {
            int attempts = 0;
            while (attempts <= 10)
            {
                node = gridController.Grid.GetRandomNode();

                if (node.Type == (int)Tiles.FREE && gridController.Grid.IsNodeInOpenRegion(node))
                {
                    break;
                }
                attempts++;
            }

            if (attempts > 10)
            {
                continue;
            }

            var enemy = Instantiate(_enemyPrefab, node.WorldPosition, Quaternion.identity);
            enemy.gridController = gridController;
            enemy.gameObject.SetActive(false);
            enemies.Add(enemy);
        }

        return enemies;
    }
    public void GenerateEnemy()
    {
        Vector2 randompoint = Random.insideUnitCircle.normalized * 2;
        Instantiate(_enemyPrefab, transform.position + (Vector3)randompoint, Quaternion.identity);
    }

    //public void GenerateEnemy(Chars.Pathfinding.Grid grid)
    //{
    //    List<Node> freeNodes = new List<Node>();

    //    foreach (var node in grid.Nodes)
    //    {
    //        if (node.Type == (int)Tiles.FREE)
    //        {
    //            freeNodes.Add(node);
    //        }
    //    }

    //    var randomIndex = Random.Range(0, freeNodes.Count);
    //    Instantiate(_enemyPrefab, freeNodes[randomIndex].WorldPosition, Quaternion.identity);
    //}
}
