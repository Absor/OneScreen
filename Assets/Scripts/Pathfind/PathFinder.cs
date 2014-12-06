using System.Collections.Generic;
using UnityEngine;

public class PathFinder {

    public static List<Vector3> FindPath(bool[][] grid, int fromX, int fromY, int toX, int toY)
    {
        List<Vector3> vertices = new List<Vector3>();
        float[][] distances = new float[grid.Length][];
        bool[][] visited = new bool[grid.Length][];
        Vector3?[][] previous = new Vector3?[grid.Length][];
        for (int x = 0; x < grid.Length; x++)
        {
            distances[x] = new float[grid[x].Length];
            previous[x] = new Vector3?[grid[x].Length];
            visited[x] = new bool[grid[x].Length];
            for (int y = 0; y < grid[x].Length; y++)
            {
                distances[x][y] = float.MaxValue;
                visited[x][y] = false;
                if (!grid[x][y]) {
                    vertices.Add(new Vector3(x, 0, y));
                }
            }            
        }
        distances[fromX][fromY] = 0;

        while (vertices.Count > 0)
        {
            Vector3 closest = Vector3.one;
            int closestX = 0, closestY = 0;
            float closestDist = float.MaxValue;
            foreach (Vector3 vertex in vertices)
            {
                int x = Mathf.RoundToInt(vertex.x);
                int y = Mathf.RoundToInt(vertex.z);
                if (distances[x][y] <= closestDist)
                {
                    closest = vertex;
                    closestX = x;
                    closestY = y;
                    closestDist = distances[x][y];
                }
            }
            vertices.Remove(closest);

            // Found target!
            if (closestX == toX && closestY == toY)
            {
                break;
            }

            visited[closestX][closestY] = true;

            float distanceToNext = distances[closestX][closestY] + 1;

            if (closestX - 1 >= 0 && !grid[closestX - 1][closestY] && !visited[closestX - 1][closestY])
            {
                if (distances[closestX - 1][closestY] > distanceToNext)
                {
                    distances[closestX - 1][closestY] = distanceToNext;
                    previous[closestX - 1][closestY] = closest;
                }
            }

            if (closestX + 1 < grid.Length && !grid[closestX + 1][closestY] && !visited[closestX + 1][closestY])
            {
                if (distances[closestX + 1][closestY] > distanceToNext)
                {
                    distances[closestX + 1][closestY] = distanceToNext;
                    previous[closestX + 1][closestY] = closest;
                }
            }

            if (closestY - 1 >= 0 && !grid[closestX][closestY - 1] && !visited[closestX][closestY - 1])
            {
                if (distances[closestX][closestY - 1] > distanceToNext)
                {
                    distances[closestX][closestY - 1] = distanceToNext;
                    previous[closestX][closestY - 1] = closest;
                }
            }

            if (closestY + 1 < grid[0].Length && !grid[closestX][closestY + 1] && !visited[closestX][closestY + 1])
            {
                if (distances[closestX][closestY + 1] > distanceToNext)
                {
                    distances[closestX][closestY + 1] = distanceToNext;
                    previous[closestX][closestY + 1] = closest;
                }
            }
        }

        List<Vector3> waypoints = new List<Vector3>();

        Vector3? target = new Vector3(toX, 0, toY);

        while (true)
        {
            Vector3? previousVec = previous[Mathf.RoundToInt(target.Value.x)][Mathf.RoundToInt(target.Value.z)];

            if (!previousVec.HasValue)
            {
                break;
            }
            waypoints.Add(target.Value);
            target = previousVec;
        }

        waypoints.Reverse();

        return waypoints;
    }
}
