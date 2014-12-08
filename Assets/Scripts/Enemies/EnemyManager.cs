using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

    [SerializeField]
    private MazeManager mazeController;
    private List<GameObject> enemiesInMaze;
    [SerializeField]
    private int maxEnemies;
    [SerializeField]
    private GameObject player;
    private bool[][] mazeGrid;
    [SerializeField]
    private GameObject[] enemyPrefabs;
    [SerializeField]
    private float minSpawnDistance;

	void Start () {
        enemiesInMaze = new List<GameObject>();
	}

    public void StartSpawningEnemies()
    {
        mazeGrid = mazeController.GetMazeGrid();
        for (int i = 0; i < maxEnemies; i++)
        {
            spawnEnemy();
        }
    }

    private void spawnEnemy()
    {
        int x, y;
        float spawnDistance = minSpawnDistance;
        do
        {
            x = Random.Range(2, mazeGrid.Length - 2);
            y = Random.Range(2, mazeGrid[0].Length - 2);
            spawnDistance -= 0.1f;
        } while (!isLocationGood(x, y, spawnDistance));

        GameObject randomEnemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]) as GameObject;
        randomEnemy.transform.SetParent(transform);
        randomEnemy.transform.position = new Vector3(x, 0, y);
        IEnemyController enemyController = randomEnemy.GetComponentInChildren(typeof(IEnemyController)) as IEnemyController;
        enemyController.StartBrain(mazeController, player, this);
        enemiesInMaze.Add(randomEnemy);
    }

    private bool isLocationGood(int x, int y, float spawnDistance)
    {
        Vector3 positionToBe = new Vector3(x, 0, y);
        bool close = false;
        foreach(GameObject enemy in enemiesInMaze) {
            if (Vector3.Distance(enemy.transform.position, positionToBe) < spawnDistance)
            {
                close = true;
            }
        }
        if (Vector3.Distance(player.transform.position, positionToBe) < spawnDistance)
        {
            close = true;
        }

        return !mazeGrid[x][y] && !close;
    }

    public void StopSpawningEnemies()
    {
        StopAllCoroutines();
        foreach (GameObject enemy in enemiesInMaze)
        {
            Destroy(enemy);
        }
        enemiesInMaze.Clear();
        mazeGrid = null;
    }

    public void KillEnemy(GameObject enemy)
    {
        Destroy(enemy);
        enemiesInMaze.Remove(enemy);
        StartCoroutine(lateSpawnEnemy());
    }

    private IEnumerator lateSpawnEnemy()
    {
        yield return new WaitForSeconds(5);
        spawnEnemy();
    }
}
