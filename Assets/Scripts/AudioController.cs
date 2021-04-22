using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip bombClip;
    public AudioClip fuelUpClip;
    public AudioClip projectileClip;
    public AudioClip timeSlowClip;
    public AudioClip explodeClip;
    public AudioClip timeBacktoNormalClip;
    public AudioClip moveClip;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(string name){
        switch (name)
        {
            case "bomb":
                audioSource.PlayOneShot(bombClip, 0.5f);
                break;
            case "explode":
                audioSource.PlayOneShot(explodeClip, 0.5f);
                break;
            case "fuelUp":
                audioSource.PlayOneShot(fuelUpClip, 0.5f);
                break;
            case "timeSlow":
                audioSource.PlayOneShot(timeSlowClip, 0.5f);
                break;
            case "timeBacktoNormal":
                audioSource.PlayOneShot(timeBacktoNormalClip, 0.5f);
                break;
            case "projectile":
                audioSource.PlayOneShot(projectileClip, 0.5f);
                break;
            case "move":
                audioSource.PlayOneShot(projectileClip, 0.5f);
                break;
        }
    }
}
