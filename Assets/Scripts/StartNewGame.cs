using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class StartNewGame : MonoBehaviour, IPointerClickHandler {

    [SerializeField]
    private GameObject startGui;
    [SerializeField]
    private MazeManager mazeManager;
    [SerializeField]
    private Help help;
    [SerializeField]
    private FlareLauncher flareLauncher;

    private float activeSeconds = 0;

    void Start()
    {
        ShowGui();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        startGui.SetActive(false);
        mazeManager.StartNewGame();
        help.Reset();
        flareLauncher.Reset();
    }

    public void ShowGui()
    {
        startGui.SetActive(true);
    }

    void OnEnable()
    {
        activeSeconds = 0;
    }

    void Update()
    {
        activeSeconds += Time.deltaTime;
        if (Input.GetAxis("Cancel") > 0 && activeSeconds > 0.5f)
        {
            Application.Quit();
        }
    }
}
