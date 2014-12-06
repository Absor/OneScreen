using UnityEngine;
using System.Collections;

public class MazeEndTrigger : MonoBehaviour {

    private MazeManager mazeController;

    public void Initialize(MazeManager mazeController)
    {
        this.mazeController = mazeController;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            mazeController.EndMaze();
        }
    }
}
