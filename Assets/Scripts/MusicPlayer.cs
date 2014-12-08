using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

    [SerializeField]
    private AudioSource[] tracks;
    private AudioSource current;

    void Start()
    {
        playNext();
    }

    void Update()
    {
        if (!current.isPlaying)
        {
            playNext();
        }
    }

    private void playNext()
    {
        current = tracks[Random.Range(1, tracks.Length)];
        current.Play();
        foreach (AudioSource track in tracks)
        {
            if (track != current)
            {
                track.Stop();
            }
        }
    }

    public void PlayWinTrack()
    {
        current = tracks[0];
        current.Play();
        foreach (AudioSource track in tracks)
        {
            if (track != current)
            {
                track.Stop();
            }
        }
    }

    public void PlayNormalMusic()
    {
        current.Stop();
    }
}
