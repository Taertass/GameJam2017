using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour {

    private static SoundHandler instance;

    public AudioClip[] jumpClips;
    public AudioClip[] impactClips;
    public AudioClip[] impactNoneStickClips;
    public AudioClip[] hurtClips;
    public AudioClip[] upgradeClips;
    public AudioClip[] musicClips;

    public AudioClip menuMusicClip;

    public AudioClip introSceneMusicClip;

    private AudioSource myAudioSource;
    private AudioSource myAudioSource2;

    private int playingMusicForLevel = 0;

    public static SoundHandler Instance
    {
        get
        {
            return instance;
        }
    }

	// Use this for initialization
	void Start () {
        instance = this;
        var audioSources = GetComponents<AudioSource>();

        myAudioSource = audioSources[0];
        myAudioSource2 = audioSources[1];

        DontDestroyOnLoad(this);
	}

    private void Update()
    {
        if(playingMusicForLevel > 0 && !myAudioSource2.isPlaying)
        {
            PlayRandomTrack();
        }
    }

    public void PlayMusicForLevel(int levelNumber)
    {
        if(levelNumber == 1)
        {
            PlayIntroSceneMusic();
        }
        else
        {
            PlayRandomTrack();
        }
        
    }

    private void PlayRandomTrack()
    {
        var clip = GetRandomClip(musicClips);
        if (clip != null)
            myAudioSource2.clip = clip;

        myAudioSource2.loop = false;
        if (!myAudioSource2.isPlaying)
            myAudioSource2.Play();
    }

    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        int numberOfTracks = 0;
        if (clips != null)
            numberOfTracks = clips.Length;
        if (numberOfTracks > 0)
        {
            var trackToPlay = UnityEngine.Random.Range(0, numberOfTracks);
            return clips[trackToPlay];
        }
        return null;
    }

    public void PlayMenuMusic()
    {
        if(menuMusicClip != null)
        {
            myAudioSource2.clip = menuMusicClip;
            myAudioSource2.loop = true;

            if (!myAudioSource2.isPlaying)
                myAudioSource2.Play();
        }
        else
        {
            PlayRandomTrack();
        }
    }

    public void PlayIntroSceneMusic()
    {
        if (menuMusicClip != null)
        {
            myAudioSource2.clip = introSceneMusicClip;
            myAudioSource2.loop = true;

            if (!myAudioSource2.isPlaying)
                myAudioSource2.Play();
        }
        else
        {
            PlayRandomTrack();
        }
    }

    public void PlayUpgradeSound()
    {
        var clip = GetRandomClip(upgradeClips);
        if (clip != null)
        {
            myAudioSource.clip = clip;
            myAudioSource.Play();
        }
    }

    public void PlayJumpClip()
    {
        var clip = GetRandomClip(jumpClips);
        if (clip != null)
        {
            myAudioSource.clip = clip;
            myAudioSource.Play();
        }
    }

    public void PlayImpactClip()
    {
        var clip = GetRandomClip(impactClips);
        if (clip != null)
        {
            myAudioSource.clip = clip;
            myAudioSource.Play();
        }
    }

    public void PlayNoneStickImpactClip()
    {
        var clip = GetRandomClip(impactNoneStickClips);
        if (clip != null)
        {
            myAudioSource.clip = clip;
            myAudioSource.Play();
        }
    }

    public void PlayHurtClip()
    {
        var clip = GetRandomClip(hurtClips);
        if (clip != null)
        {
            myAudioSource.clip = clip;
            myAudioSource.Play();
        }
    }
}
