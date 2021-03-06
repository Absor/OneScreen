﻿using UnityEngine;
using System.Collections;

public class RockController : MonoBehaviour, IEnemyController {

    private bool triggered = false;
    private bool playerSees = false;
    private int xSpeed, ySpeed;
    [SerializeField]
    private float movementForce;
    private bool moving = false;
    private float lastMovement = 0;
    private EnemyManager enemyManager;
    private AudioSource sound;
    private GameObject player;

    void Start()
    {
        sound = GetComponentInChildren<AudioSource>();
    }

    public void StartBrain(MazeManager mazeController, GameObject player, EnemyManager enemyManager)
    {
        bool[][] mazeGrid = mazeController.GetMazeGrid();
        this.enemyManager = enemyManager;
        this.player = player;
        int startX = Mathf.RoundToInt(transform.position.x);
        int startY = Mathf.RoundToInt(transform.position.z);
        int xCount = 0, yCount = 0;

        // X neg
        int x = startX - 1;
        while (true)
        {
            if (x < 0 || mazeGrid[x][startY])
            {
                break;
            }
            xCount++;
            x--;
        }
        // X pos
        x = startX + 1;
        while (true)
        {
            if (x >= mazeGrid.Length || mazeGrid[x][startY])
            {
                break;
            }
            xCount++;
            x++;
        }
        // Y neg
        int y = startY - 1;
        while (true)
        {
            if (y < 0 || mazeGrid[startX][y])
            {
                break;
            }
            yCount++;
            y--;
        }
        // X pos
        y = startY + 1;
        while (true)
        {
            if (y >= mazeGrid[0].Length || mazeGrid[startX][y])
            {
                break;
            }
            yCount++;
            y++;
        }

        if (xCount > yCount)
        {
            xSpeed = 1;
            ySpeed = 0;
        }
        else
        {
            xSpeed = 0;
            ySpeed = 1;
        }
    }

    void Update()
    {
        if (!moving)
        {
            lastMovement += Time.deltaTime;
            if (lastMovement > 1)
            {
                xSpeed = xSpeed * -1;
                ySpeed = ySpeed * -1;
                lastMovement = 0;
            }
        }
        if (moving && rigidbody.velocity.magnitude < 0.1f)
        {
            lastMovement = 0;
            moving = false;
        }
        else if (!moving && rigidbody.velocity.magnitude > 0.1f)
        {
            moving = true;
        }

        float forceCoeff = Time.timeScale * movementForce;
        rigidbody.AddForce(new Vector3(xSpeed, 0, ySpeed) * forceCoeff);
        sound.volume = Mathf.Clamp(rigidbody.velocity.magnitude, 0, 1);
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
            player.GetComponent<PlayerController>().SayWarning("rock");
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
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && rigidbody.velocity.magnitude > 0.2f)
        {
            player.GetComponent<PlayerController>().Die("rock");
        }
    }
}
