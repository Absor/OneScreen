using UnityEngine;
using System.Collections;

public class FlareController : MonoBehaviour {

    [SerializeField]
    private float ascendTime;
    [SerializeField]
    private float lifeTime;

	void Start () {
        StartCoroutine(ascend());
	}

    private IEnumerator ascend()
    {
        float t = 0.0f;
        Vector3 from = transform.position;
        Vector3 to = transform.position + Vector3.up * 5;

        while (t < 1.0f)
        {
            t += Time.unscaledDeltaTime  / ascendTime;
            transform.position = Vector3.Lerp(from, to, t);
            light.intensity = Mathf.Lerp(0.1f, 0.5f, t);
            yield return 0;
        }

        t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.unscaledDeltaTime * lifeTime;
            light.intensity = Mathf.Lerp(0.5f, 0f, t);
            yield return 0;
        }

        Destroy(gameObject);
    }
}
