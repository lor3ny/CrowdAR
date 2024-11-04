using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class ZombieAudio : MonoBehaviour
{
    public List<AudioClip> zombieClips;   // Regular zombie sounds to play in loop
    public AudioClip deathSound;          // Sound to play once when zombie dies
    private AudioSource audioSource;
    private bool hasPlayedDeathSound = false;

    [Range(0f, 1f)]
    public float regularVolume = 0.1f;    // Lower volume for ambient sounds
    [Range(0f, 1f)]
    public float deathSoundVolume = 1.0f; // Higher volume for death sound

    private PlayerController playerController;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("Missing AudioSource on zombie prefab.");
            return;
        }

        audioSource.volume = regularVolume;  
        playerController = GetComponentInParent<PlayerController>();

        if (zombieClips.Count > 0 && playerController != null)
        {
            ZombieManager.Instance.RegisterZombie(playerController);
            StartCoroutine(PlayRandomZombieSounds());
        }
        else
        {
            Debug.LogWarning("Zombie clips, AudioSource, or PlayerController not set on " + gameObject.name);
        }
    }

    private IEnumerator PlayRandomZombieSounds()
    {
        while (true)
        {
            if (playerController != null)
            {
                // Stop all sounds if all zombies are in WINNER or DEAD states
                if (ZombieManager.Instance.AllZombiesInWinnerOrDeadState())
                {
                    audioSource.Stop();
                    yield break; 
                }

                // Play ambient sounds only if the zombie's state is not DEAD
                if (playerController.plState != PlayerState.DEAD)
                {
                    hasPlayedDeathSound = false;  

                    
                    if (!audioSource.isPlaying)
                    {
                        AudioClip clipToPlay = zombieClips[Random.Range(0, zombieClips.Count)];
                        audioSource.clip = clipToPlay;
                        audioSource.volume = regularVolume;
                        audioSource.Play();

                        yield return new WaitForSeconds(clipToPlay.length + Random.Range(2f, 4f));
                    }
                    else
                    {
                        yield return null;  
                    }
                }
                else
                {
                    // Play death sound immediately if the zombie just died and hasn't played the death sound yet
                    if (!hasPlayedDeathSound)
                    {
                        audioSource.Stop(); 
                        audioSource.PlayOneShot(deathSound, deathSoundVolume * 10.0f);  
                        hasPlayedDeathSound = true;

                    }
                    else
                    {
                        yield return null;
                    }
                }
            }
            else
            {
                yield return null;
            }
        }
    }
}
