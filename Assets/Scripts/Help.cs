using UnityEngine;
using System.Collections;

public class Help : MonoBehaviour {

    [SerializeField]
    private PlayerMessage playerMessage;
    [SerializeField]
    private float messageDuration;
    [SerializeField]
    private float[] timesBetweenMessages;
    [SerializeField]
    private string[] messages;

    private int index = 0;

	void Start () {
        showNextMessage();
    }

    private void showNextMessage()
    {
        if (timesBetweenMessages.Length > index && messages.Length > index)
        {
            StartCoroutine(showMessage(messages[index], timesBetweenMessages[index]));
            index++;
        }
    }

    private IEnumerator showMessage(string message, float delay)
    {
        yield return new WaitForSeconds(delay);
        playerMessage.ShowMessage(message, messageDuration);
        showNextMessage();
    }

    public void Reset()
    {
        StopAllCoroutines();
        index = 0;
        showNextMessage();
    }
}
