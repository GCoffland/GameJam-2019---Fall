using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private void Start()
    {
        instance = this;
    }

    public AudioSource fire;
    public AudioClip fireClip;
    public AudioClip tinkClip;
    public AudioClip teleportClip;
    public AudioClip stunClip;
    public AudioClip dieClip;

    public void Fire()
    {
        fire.PlayOneShot(fireClip, 0.75f);
    }

    public void Tink()
    {
        fire.PlayOneShot(tinkClip, 0.55f);
    }

    public void Teleport()
    {
        fire.PlayOneShot(teleportClip, 1.0f);
    }

    public void Stun()
    {
        fire.PlayOneShot(stunClip, 1.0f);
    }

    public void Die()
    {
        fire.PlayOneShot(dieClip, 1.0f);
    }
}
