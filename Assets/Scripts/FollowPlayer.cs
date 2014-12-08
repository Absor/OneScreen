using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    [SerializeField]
    private GameObject player;

	void Update () {
        transform.position = new Vector3(player.transform.position.x, 1.1f, player.transform.position.z);
	}
}
