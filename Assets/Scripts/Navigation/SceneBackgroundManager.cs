using UnityEngine;

public class SceneBackgroundMusic : MonoBehaviour
{
    [Header("Music Settings")]
    public AudioClip musicClip;
    [Range(0f, 1f)] public float volume = 0.4f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        audioSource.Play();
    }
}
