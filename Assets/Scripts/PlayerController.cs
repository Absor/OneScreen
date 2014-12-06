using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float forwardSpeed;
    [SerializeField]
    private AudioSource deathSound;
    [SerializeField]
    private MazeManager mazeManager;
    private bool dead = false;

	void Update () {
        if (dead)
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

    public void Die()
    {
        if (dead)
        {
            return;
        }
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
}
