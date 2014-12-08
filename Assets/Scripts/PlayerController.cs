using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float forwardSpeed;
    [SerializeField]
    private AudioSource deathSound;
    [SerializeField]
    private MazeManager mazeManager;
    private bool dead = false;
    [SerializeField]
    private PlayerMessage playerMessage;
    private string killer;

    private HashSet<string> seen = new HashSet<string>();

	void Update () {
        if (dead || Time.timeScale == 0)
        {
            return;
        }
        // Mouse look
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float distance;
        plane.Raycast(ray, out distance);
        Vector3 mouseLocation = ray.GetPoint(distance);
        transform.rotation = Quaternion.LookRotation(mouseLocation - transform.position);

        // Vertical movement
        float verticalSpeed = Input.GetAxis("Fire1");
        if (verticalSpeed > 0)
        {
            verticalSpeed = verticalSpeed * forwardSpeed;
        }
        verticalSpeed = verticalSpeed * Time.deltaTime;
        rigidbody.velocity = transform.forward * verticalSpeed;
	}

    public void Die(string killerName)
    {
        if (dead)
        {
            return;
        }
        killer = killerName;
        dead = true;
        rigidbody.constraints = RigidbodyConstraints.None;
        deathSound.Play();

        StartCoroutine(lateRevive());
    }

    private IEnumerator lateRevive()
    {
        yield return new WaitForSeconds(1f);
        Revive();
    }

    public void Revive()
    {
        dead = false;
        transform.rotation = Quaternion.identity;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        mazeManager.StartNewRound();
    }

    public void SayWhoKilledMe()
    {
        if (killer != null)
        {
            playerMessage.ShowMessage("Bah! Killed by a " + killer + ".", 2f);
            killer = null;
        }
    }

    public void SayWarning(string what)
    {
        if (!seen.Contains(what))
        {
            seen.Add(what);
            playerMessage.ShowMessage("Watch out!", 1f);

            if (what == "spider")
            {
                StartCoroutine(lateSay("I hate spiders.", 2f, 1f));
            }
            else if (what == "trap")
            {
                StartCoroutine(lateSay("I can sneak past holes in the floor.", 2f, 2f));
            }
            else if (what == "snake")
            {
                StartCoroutine(lateSay("Snakes. Why'd it have to be snakes?", 2f, 2f));
            }
            else if (what == "rock")
            {
                StartCoroutine(lateSay("Those boulders rarely stop moving.", 2f, 2f));
            } 
        }
    }

    private IEnumerator lateSay(string p1, float delay, float duration)
    {
        yield return new WaitForSeconds(delay);
        playerMessage.ShowMessage(p1, duration);
    }

    public void SaySuccess()
    {
        string[] messages = { "One step closer to success.", "I will survive!", "Easy!" };
        playerMessage.ShowMessage(messages[Random.Range(0, messages.Length)], 2f);
    }

    public void SayLastLevel()
    {
        string[] messages = { "This must be the last one.", "One more!", "I can see the end from here!" };
        playerMessage.ShowMessage(messages[Random.Range(0, messages.Length)], 2f);
    }
}
