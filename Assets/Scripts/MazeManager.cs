using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wall
{
    public int x;
    public int y;
    public int oppositeX;
    public int oppositeY;

    public Wall(int x, int y, int oppositeX, int oppositeY)
    {
        this.x = x;
        this.y = y;
        this.oppositeX = oppositeX;
        this.oppositeY = oppositeY;
    }
}

public class MazeManager : MonoBehaviour {

    [SerializeField]
    private GameObject wallPrefab;
    [SerializeField]
    private GameObject doorPrefab;
    [SerializeField]
    private int sizeX;
    [SerializeField]
    private int sizeY;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private EnemyManager enemyController;
    [SerializeField]
    private float playerMoveDuration;

    private int mazeStartX, mazeStartY, mazeEndX, mazeEndY;

    private bool[][] mazeGrid;

	void Start () {
        sizeX = Mathf.Max(sizeX, 17);
        sizeY = Mathf.Max(sizeY, 9);
        if (sizeX % 2 == 0)
        {
            sizeX++;
        }
        if (sizeY % 2 == 0)
        {
            sizeY++;
        }

        mazeEndX = Mathf.RoundToInt(sizeX / 2);
        mazeEndY = sizeY - 1;
        mazeStartX = mazeEndX;
        mazeStartY = 0;

        startNewRound();
	}

    public bool[][] GetMazeGrid()
    {
        return mazeGrid;
    }

    private void startNewRound() {
        Time.timeScale = 0;
        createNewMaze();
        enemyController.StartSpawningEnemies();
        StartCoroutine(movePlayer(new Vector3(mazeStartX, 0, mazeStartY), getPlayerStartPosition(), 2f, mazeStarted));
    }

    private void mazeStarted()
    {
        Time.timeScale = 1;
        Light flashLight = player.GetComponentInChildren<Light>();
        flashLight.enabled = true;
    }

    private Vector3 getPlayerStartPosition()
    {
        return new Vector3(mazeStartX, 0, mazeStartY + 1);
    }

    private void createNewMaze()
    {
        // Clean old
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // New grid
        createMazeGrid();

        // Create new blocks
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (mazeGrid[x][y])
                {
                    GameObject wall = Instantiate(wallPrefab) as GameObject;
                    wall.transform.SetParent(transform);
                    wall.transform.position = new Vector3(x, 0, y);
                    wall.name = x + ", " + y;
                }
            }
        }

        GameObject startDoor = Instantiate(doorPrefab) as GameObject;
        startDoor.transform.SetParent(transform);
        startDoor.transform.position = new Vector3(mazeStartX, 0, mazeStartY);
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = new Vector3(0, 0, 0);
        startDoor.transform.rotation = rotation;
        startDoor.name = "Start door";

        mazeGrid[mazeStartX][mazeStartY] = true;

        GameObject endDoor = Instantiate(doorPrefab) as GameObject;
        endDoor.transform.SetParent(transform);
        endDoor.transform.position = new Vector3(mazeEndX, 0, mazeEndY);
        rotation = Quaternion.identity;
        rotation.eulerAngles = new Vector3(0, 180, 0);
        endDoor.transform.rotation = rotation;
        endDoor.name = "End door";
        MazeEndTrigger endTrigger = endDoor.AddComponent<MazeEndTrigger>();
        endTrigger.Initialize(this);

        mazeGrid[mazeEndX][mazeEndY] = true;
    }

    // Random Prim
    private void createMazeGrid()
    {
        // Start with a grid full of walls.
        mazeGrid = new bool[sizeX][];
        for (int x = 0; x < sizeX; x++)
        {
            mazeGrid[x] = new bool[sizeY];
            for (int y = 0; y < sizeY; y++)
            {
                mazeGrid[x][y] = true;
            }
        }
        // Pick a cell, mark it as part of the maze. Add the walls of the cell to the wall list.
        List<Wall> walls = new List<Wall>();
        mazeGrid[mazeStartX][mazeStartY] = false;
        addWalls(mazeStartX, mazeStartY, walls, mazeGrid);
        // While there are walls in the list:
        while (walls.Count > 0)
        {
            // Pick a random wall from the list. If the cell on the opposite side isn't in the maze yet:
            int randomIndex = Random.Range(0, walls.Count);
            Wall randomWall = walls[randomIndex];
            if (isInsideGrid(randomWall.oppositeX, randomWall.oppositeY) && mazeGrid[randomWall.oppositeX][randomWall.oppositeY])
            {
                // Make the wall a passage and mark the cell on the opposite side as part of the maze.
                mazeGrid[randomWall.x][randomWall.y] = false;
                mazeGrid[randomWall.oppositeX][randomWall.oppositeY] = false;
                // Add the neighboring walls of the cell to the wall list.
                addWalls(randomWall.oppositeX, randomWall.oppositeY, walls, mazeGrid);
            }
            // Remove the wall from the list.
            walls.Remove(randomWall);
        }

        int removeY = mazeEndY;
        while (true)
        {
            if (mazeGrid[mazeEndX][removeY])
            {
                mazeGrid[mazeEndX][removeY] = false;
            }
            else
            {
                break;
            }
            removeY--;
        }
    }

    private bool isInsideGrid(int x, int y)
    {
        return x > 0 && y > 0 && x < sizeX - 1 && y < sizeY - 1;
    }

    private void addWalls(int x, int y, List<Wall> walls, bool[][] grid)
    {
        if (x - 1 >= 0 && grid[x - 1][y])
        {
            walls.Add(new Wall(x - 1, y, x - 2, y));
        }
        if (y - 1 >= 0 && grid[x][y - 1])
        {
            walls.Add(new Wall(x, y - 1, x, y - 2));
        }
        if (x + 1 < sizeX && grid[x + 1][y])
        {
            walls.Add(new Wall(x + 1, y, x + 2, y));
        }
        if (y + 1 < sizeY && grid[x][y + 1])
        {
            walls.Add(new Wall(x, y + 1, x, y + 2));
        }
    }

    public void EndMaze()
    {
        Time.timeScale = 0;
        Light flashLight = player.GetComponentInChildren<Light>();
        flashLight.enabled = false;
        StartCoroutine(movePlayer(player.transform.position, new Vector3(mazeEndX, 0, mazeEndY), 2f, mazeEnded));
    }

    private void mazeEnded()
    {
        enemyController.StopSpawningEnemies();
        Time.timeScale = 1;
        startNewRound();
    }

    private IEnumerator movePlayer(Vector3 from, Vector3 to, float duration, System.Action done)
    {
        float t = 0.0f;
        Quaternion rotation = Quaternion.LookRotation(to - from, Vector3.up);
        while (t < 1.0f)
        {
            t += Time.unscaledDeltaTime / duration;
            player.transform.position = Vector3.Lerp(from, to, t);
            player.transform.rotation = rotation;
            yield return 0;
        }

        done();
    }
}
