using UnityEngine;

public interface IEnemyController
{
    void StartBrain(bool[][] mazeGrid, GameObject player, EnemyManager enemyManager);
}
