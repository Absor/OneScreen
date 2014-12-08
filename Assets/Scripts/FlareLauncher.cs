using UnityEngine;
using System.Collections;

public class FlareLauncher : MonoBehaviour {

    [SerializeField]
    private GameObject flarePrefab;
    [SerializeField]
    private float cooldown;
    [SerializeField]
    private PlayerMessage playerMessage;

    private bool messaged = false;

    private float timeSinceLastLaunch = 0;

    void Start()
    {
        Reset();
    }

	void Update () {
        timeSinceLastLaunch += Time.timeScale * Time.deltaTime;

	    if (Input.GetAxis("Fire2") > 0 && timeSinceLastLaunch > cooldown) {
            timeSinceLastLaunch = 0;
            GameObject flare = Instantiate(flarePrefab) as GameObject;
            flare.transform.position = transform.position + Vector3.up * 0.5f;
            playerMessage.ShowMessage("Bang!", 1f);
        }
        else if (!messaged && Input.GetAxis("Fire2") > 0 && timeSinceLastLaunch < cooldown && timeSinceLastLaunch > 1)
        {
            playerMessage.ShowMessage("My flare gun needs a minute to cool down.", 3f);
            messaged = true;
        }
	}

    public void Reset()
    {
        timeSinceLastLaunch = cooldown;
    }
}
