using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EndGame : MonoBehaviour, IPointerClickHandler
{

    [SerializeField]
    private GameObject endGui;
    [SerializeField]
    private MusicPlayer musicPlayer;
    [SerializeField]
    private StartNewGame startNewGame;

    void Start()
    {
        endGui.SetActive(false);
    }

    public void ShowGui(float time)
    {
        endGui.SetActive(true);
        Time.timeScale = 0;
        musicPlayer.PlayWinTrack();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        endGui.SetActive(false);
        Time.timeScale = 1;
        startNewGame.ShowGui();
        musicPlayer.PlayNormalMusic();
    }
}
