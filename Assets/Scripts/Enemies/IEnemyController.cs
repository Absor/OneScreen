using UnityEngine;

public interface IEnemyController
{
    void StartBrain(MazeManager mazeController, GameObject player, EnemyManager enemyManager);
}
