using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpiderController : MonoBehaviour, IEnemyController {

    private bool triggered = false;
    private bool playerSees = false;
    [SerializeField]
    private AudioSource aggroSound;
    private EnemyManager enemyManager;
    private GameObject player;
    private bool[][] mazeGrid;
    private List<Vector3> waypoints;
    private Vector3? currentWaypoint;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float deathTime;

    void Update()
    {
        if (currentWaypoint.HasValue)
        {
            float newSpeed = speed * Time.deltaTime;
            Vector3 direction = (currentWaypoint.Value - transform.position).normalized;
            rigidbody.velocity = direction * newSpeed;

            if (Vector3.Distance(transform.position, currentWaypoint.Value) < 0.05f)
            {
                currentWaypoint = null;
            }
        } 
        if (!triggered)
        {
            return;
        }
        if (!currentWaypoint.HasValue && waypoints.Count > 0) {
            currentWaypoint = waypoints[0];
            waypoints.Remove(waypoints[0]);
        }
        if (waypoints.Count == 0)
        {
            triggered = false;
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

        Vector3 from = transform.position + Vector3.up * 0.5f;
        Vector3 to = player.transform.position + Vector3.up * 0.5f;
        Ray ray = new Ray(from, to - from);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "Player")
        {
            StartCoroutine(die());
            triggered = true;
            aggroSound.Play();
            int fromX = Mathf.RoundToInt(transform.position.x);
            int fromY = Mathf.RoundToInt(transform.position.z);
            int toX = Mathf.RoundToInt(player.transform.position.x);
            int toY = Mathf.RoundToInt(player.transform.position.z);
            waypoints = PathFinder.FindPath(mazeGrid, fromX, fromY, toX, toY);
        }
    }

    public IEnumerator die()
    {
        while (true)
        {
            yield return new WaitForSeconds(deathTime);
            if (!playerSees)
            {
                enemyManager.KillEnemy(gameObject);
                break;
            }
            playerSees = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerController>().Die();
        }
    }

    public void StartBrain(MazeManager mazeController, GameObject player, EnemyManager enemyManager)
    {
        this.enemyManager = enemyManager;
        this.player = player;
        this.mazeGrid = mazeController.GetMazeGrid();
    }
}
