using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float forwardSpeed;

	void Update () {
        // Mouse look
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float distance;
        plane.Raycast(ray, out distance);
        Vector3 mouseLocation = ray.GetPoint(distance);
        transform.rotation = Quaternion.LookRotation(mouseLocation - transform.position);

        // Vertical movement
        float verticalSpeed = Input.GetAxis("Vertical");
        if (verticalSpeed > 0)
        {
            verticalSpeed = verticalSpeed * forwardSpeed;
        }
        //else
        //{
        //    verticalSpeed = verticalSpeed * forwardSpeed;
        //}
        verticalSpeed = verticalSpeed * Time.deltaTime;
        rigidbody.velocity = transform.forward * verticalSpeed;

        // Horizontal movement
        // TODO float horizontalSpeed = Input.GetAxis("Horizontal");
	}
}
