using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LifePanelController : MonoBehaviour {

    [SerializeField]
    private GameObject lifePrefab;
    private List<GameObject> lifes;

	void Start () {
        lifes = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            GameObject life = Instantiate(lifePrefab) as GameObject;
            life.transform.SetParent(transform);
            lifes.Add(life);
        }
	}
}
