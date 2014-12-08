using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerMessage : MonoBehaviour {

    [SerializeField]
    private Text textField;
    private bool showing = false;
    private List<string> messageQueue;
    private Dictionary<string, float> durations;

	void Start () {
        messageQueue = new List<string>();
        durations = new Dictionary<string, float>();
        gameObject.SetActive(false);
	}

    public void ShowMessage(string text, float duration)
    {
        if (showing)
        {
            if (!durations.ContainsKey(text))
            {
                messageQueue.Add(text);
                durations.Add(text, duration);
            }
            return;
        }
        showing = true;
        gameObject.SetActive(true);
        textField.text = text;
        StartCoroutine(lateDisable(duration));
    }

    private IEnumerator lateDisable(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
        showing = false;
        if (messageQueue.Count > 0)
        {
            string message = messageQueue[0];
            messageQueue.Remove(message);
            float otherDuration = durations[message];
            durations.Remove(message);
            ShowMessage(message, otherDuration);
        }
    }
}
