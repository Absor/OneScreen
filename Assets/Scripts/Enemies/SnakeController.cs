using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnakeController : MonoBehaviour, IEnemyController {

    private Vector3 next;
    private Vector3 last;
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private AudioSource aggroSound;
    private bool triggered = false;
    private bool playerSees = false;
    private GameObject player;
    private EnemyManager enemyManager;

    private bool[][] mazeGrid;

    void Start()
    {
        findNextTarget();
        StartCoroutine(randomSounds());
    }

    private IEnumerator randomSounds()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 15f));
            aggroSound.Play();
        }
    }

	void Update () {
        if (Vector3.Distance(transform.position, next) < 0.1f)
        {
            findNextTarget();
        }

        float speed = movementSpeed * Time.deltaTime;
        Vector3 direction = (next - transform.position).normalized;
        rigidbody.velocity = direction * speed;
	}

    private void findNextTarget()
    {
        List<Vector3> possibleMovements = new List<Vector3>();
        
        int currentX = Mathf.RoundToInt(transform.position.x);
        int currentY = Mathf.RoundToInt(transform.position.z);

        if (currentX - 1 > 0 && !mazeGrid[currentX - 1][currentY])
        {
            possibleMovements.Add(new Vector3(currentX - 1, 0, currentY));
        }

        if (currentX + 1 < mazeGrid.Length && !mazeGrid[currentX + 1][currentY])
        {
            possibleMovements.Add(new Vector3(currentX + 1, 0, currentY));
        }

        if (currentY - 1 > 0 && !mazeGrid[currentX][currentY - 1])
        {
            possibleMovements.Add(new Vector3(currentX, 0, currentY - 1));
        }

        if (currentY + 1 < mazeGrid[0].Length && !mazeGrid[currentX][currentY + 1])
        {
            possibleMovements.Add(new Vector3(currentX, 0, currentY + 1));
        }

        if (possibleMovements.Count == 1)
        {
            last = next;
            next = possibleMovements[0];
        }
        else if (possibleMovements.Count > 1)
        {
            Vector3 random;
            do
            {
                random = possibleMovements[Random.Range(0, possibleMovements.Count)];
            } while (random == last);
            last = next;
            next = random;
        }

        transform.LookAt(next);
    }

    public void StartBrain(MazeManager mazeController, GameObject player, EnemyManager enemyManager)
    {
        mazeGrid = mazeController.GetMazeGrid();
        this.enemyManager = enemyManager;
        this.player = player;
    }

    void OnTriggerEnter(Collider other)
    {
        OnTriggerStay(other);
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
            player.GetComponent<PlayerController>().SayWarning("snake");
            aggroSound.Play();
        }
    }

    public IEnumerator die()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
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
        if (collision.gameObject.tag == "Player" && rigidbody.velocity.magnitude > 0.2f)
        {
            player.GetComponent<PlayerController>().Die("snake");
        }
    }
}
