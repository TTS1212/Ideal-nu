using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Footsteps : MonoBehaviour
{
    // Audio

    [SerializeField] AudioClip[] footsteps;
    private AudioSource _source;

    private void Awake()
    {
        _source = this.gameObject.AddComponent<AudioSource>();
    }

    public void FootstepsSound()
    {
        int random = Random.Range(0, footsteps.Length);
        _source.PlayOneShot(footsteps[random]);
    }
}
