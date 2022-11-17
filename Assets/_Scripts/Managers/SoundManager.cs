using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] AudioSource stepSound;

    [SerializeField] AudioSource deathSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayStepSound()
    {
        if (stepSound != null)
        {
            if (stepSound.isPlaying)
            {
                stepSound.Stop();
            }

            stepSound.Play();
        }
    }

    public void PlayDeathSound()
    {
        if (deathSound != null)
        {
            if (deathSound.isPlaying)
            {
                deathSound.Stop();
            }

            deathSound.Play();
        }
    }
}
