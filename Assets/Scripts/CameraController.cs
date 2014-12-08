using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private MazeManager mazeManager;
	
	void Update () {
	    transform.position = new Vector3(1.0f * mazeManager.GetSizeX() / 2, transform.position.y, 1.0f * mazeManager.GetSizeY() / 2);
	}
}
