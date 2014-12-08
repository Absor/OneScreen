using UnityEngine;
using System.Collections;

public class TrapController : MonoBehaviour, IEnemyController {

    private bool playerSees = false;
    private bool triggered = false;
    private int myX, myY;
    private GameObject player;
    private PlayerController playerController;
    private EnemyManager enemyManager;
    private MazeManager mazeManager;

	public void StartBrain(MazeManager mazeManager, GameObject player, EnemyManager enemyManager)
    {
        this.mazeManager = mazeManager;
        this.enemyManager = enemyManager;
        myX = Mathf.RoundToInt(transform.position.x);
        myY = Mathf.RoundToInt(transform.position.z);
        mazeManager.DisableFloorTile(myX, myY);
        transform.position -= Vector3.up;
        this.player = player;
        this.playerController = player.GetComponent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerController.Die("trap");
        }
        else
        {
            OnTriggerStay(other);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "PlayerView")
        {
            return;
        }
        playerSees = true;
        if (triggered)
        {
            return;
        }

        Vector3 from = transform.position + Vector3.up * 1.5f;
        Vector3 to = player.transform.position + Vector3.up * 0.5f;
        Ray ray = new Ray(from, to - from);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "Player")
        {
            StartCoroutine(die());
            triggered = true;
            playerController.SayWarning("trap");
        }
    }

    public IEnumerator die()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            if (!playerSees)
            {
                enemyManager.KillEnemy(gameObject);
                break;
            }
            playerSees = false;
        }
        mazeManager.EnableFloorTile(myX, myY);
    }
}
