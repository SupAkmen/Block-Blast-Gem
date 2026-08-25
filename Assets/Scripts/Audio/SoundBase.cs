using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class SoundBase : SingletonBehaviour<SoundBase>
{
     [SerializeField] private AudioMixer mixer;
     [SerializeField] private string soundParameter = "soundVolume";

     public AudioClip click;
     public AudioClip[] swish;
     public AudioClip coins;
     public AudioClip coinsSpend;
     public AudioClip luckySpin;
     public AudioClip warningTime;
     public AudioClip placeShape;
     public AudioClip fillEmpty;
     public AudioClip alert;
     public AudioClip[] combo;
     
     private AudioSource audioSource;

     private readonly HashSet<AudioClip> clipPlaying = new();

     public override void Awake()
     {
          base.Awake();
          audioSource = GetComponent<AudioSource>();
     }

     private void Start()
     {
          mixer.SetFloat(soundParameter, PlayerPrefs.GetInt("Sound", 1) == 0 ? -80 : 0);
     }

     public void PlaySound(AudioClip clip)
     {
          if (clip != null)
          {
               audioSource.PlayOneShot(clip);
          }
     }

     public void PlayDelayed(AudioClip clip, float delay)
     {
          StartCoroutine((PlayDelayedCoroutine(clip, delay)));
     }

     private IEnumerator PlayDelayedCoroutine(AudioClip clip, float delay)
     {
          yield return new WaitForSeconds(delay);
          PlaySound(clip);
     }

     public void PlaySoundRandom(AudioClip[] clip)
     {
          instance.PlaySound(clip[Random.Range(0, clip.Length)]);
     }

     public void PlayLimitSound(AudioClip clip)
     {
          if (clipPlaying.Add(clip))
          {
               PlaySound(clip);
               StartCoroutine(WaitForCompleteSound(clip));
          }
     }

     private IEnumerator WaitForCompleteSound(AudioClip clip)
     {
          yield return new WaitForSeconds(0.1f);
          clipPlaying.Remove(clip);
     }
}
