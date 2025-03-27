using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource environmentSource;
    public AudioClip[] randomScarySounds;

    public float minTimeBetweenSounds = 30f;
    public float maxTimeBetweenSounds = 180f;

    private float nextSoundTime;

    void Start()
    {
        ScheduleNextSound();
    }

    void Update()
    {
        if (Time.time >= nextSoundTime)
        {
            PlayRandomScarySound();
            ScheduleNextSound();
        }
    }

    void PlayRandomScarySound()
    {
        if (randomScarySounds.Length > 0)
        {
            int randomIndex = Random.Range(0, randomScarySounds.Length);
            environmentSource.PlayOneShot(randomScarySounds[randomIndex]);
        }
    }

    void ScheduleNextSound()
    {
        nextSoundTime = Time.time + Random.Range(minTimeBetweenSounds, maxTimeBetweenSounds);
    }
}
