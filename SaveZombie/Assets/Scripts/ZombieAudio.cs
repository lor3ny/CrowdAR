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

        StartCoroutine(PlayRandomZombieSounds());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayedDeathSound)
            return;

        if (other.CompareTag("Death") || other.CompareTag("Rock"))
        {
            if (IsCollidingWithBridge())
            {
                // If colliding with the bridge, do not set the state to DEAD
                Debug.Log("Player is safe on the bridge!");
                return; // Exit the method to prevent death
            }
            Debug.Log("here");
            audioSource.Stop();
            audioSource.PlayOneShot(deathSound, deathSoundVolume * 10.0f);
            hasPlayedDeathSound = true;
        }
    }
    private bool IsCollidingWithBridge()
    {
        // Check if the player is colliding with an object tagged as "Bridge"
        // You may need to modify this based on your specific bridge tag
        return Physics.CheckSphere(transform.position, 0.5f, LayerMask.GetMask("modelObject"));
    }

    private IEnumerator PlayRandomZombieSounds()
    {
        while (true)
        {
            if (playerController == null)
                yield return null;

            if (hasPlayedDeathSound)
                yield return null;

            if(playerController.plState == PlayerState.WINNER)
                yield return null;

            if (playerController.plState== PlayerState.DEAD)
                yield return null;

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
    }
}
