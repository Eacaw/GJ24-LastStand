using UnityEngine;
using System.Collections.Generic;

public class AudioPoolManager : MonoBehaviour
{
    public static AudioPoolManager Instance;

    public AudioSource audioSourcePrefab;
    public int poolSize = 10;

    private List<AudioSource> audioSources;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        audioSources = new List<AudioSource>();

        for (int i = 0; i < poolSize; i++)
        {
            AudioSource newAudioSource = Instantiate(audioSourcePrefab, transform);
            newAudioSource.gameObject.SetActive(false);
            audioSources.Add(newAudioSource);
        }
    }

    public void PlaySound(AudioClip clip, Vector3 position)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.transform.position = position;
                audioSource.clip = clip;
                audioSource.gameObject.SetActive(true);
                audioSource.Play();
                StartCoroutine(DeactivateAudioSource(audioSource));
                return;
            }
        }

        // If all audio sources are busy, create a new one
        AudioSource newAudioSource = Instantiate(audioSourcePrefab, position, Quaternion.identity, transform);
        newAudioSource.clip = clip;
        newAudioSource.Play();
        audioSources.Add(newAudioSource);
        StartCoroutine(DeactivateAudioSource(newAudioSource));
    }

    private IEnumerator<WaitForSeconds> DeactivateAudioSource(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSource.gameObject.SetActive(false);
    }
}
