using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _Instance;

    public static AudioManager Instance
    {
        get
        {
            if(_Instance == null)
            {
                _Instance = GameObject.FindObjectOfType<AudioManager>();
            }
            return _Instance;
        }
    }
    [SerializeField] private AudioClip Gameplay;
    [SerializeField] private AudioClip Victory;
    [SerializeField] private AudioClip GameOver;

    public AudioSource CurrentAudioSource { get; private set; }

    private void Awake()
    {
        if (CurrentAudioSource == null) CurrentAudioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        PlayGameplay();
    }
    private void OnEnable()
    {
        Player.OnGameOver += PlayGameOver;
        Player.OnVictory += PlayVictory;
    }
    private void OnDisable()
    {
        Player.OnGameOver -= PlayGameOver;
        Player.OnVictory -= PlayVictory;
    }
    public void PlayClip(AudioClip clip, bool isLooping)
    {
        CurrentAudioSource.loop = isLooping;
        CurrentAudioSource.clip = clip;
        CurrentAudioSource.Play();
    }
    //Method to call when one shot
    public void PlayOneClip(AudioClip clip)
    {
        if (clip == null) return;
        CurrentAudioSource.PlayOneShot(clip);
    }
    public IEnumerator WaitForEndOfSong(AudioClip clip, bool isLooping = false)
    {
        PlayClip(clip, isLooping);
        yield return new WaitUntil( () => !CurrentAudioSource.isPlaying);
    }
    private void PlayGameplay()
    {
        PlayClip(Gameplay, true);
    }
    private void PlayGameOver(bool isCalled)
    {
        if (isCalled)
        {
            PlayClip(GameOver, false);
        }
        else
        {
            CurrentAudioSource.Stop();
        }
    }
    private void PlayVictory(bool isCalled)
    {
        if (isCalled)
        {
            PlayClip(Victory, false);
        }
        else
        {
            CurrentAudioSource.Stop();
        }
    }
}
