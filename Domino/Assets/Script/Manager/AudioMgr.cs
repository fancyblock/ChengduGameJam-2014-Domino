using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AudioMgr: MonoBehaviour{

	public AudioClip[] sfxClips;

	public AudioClip[] bgmClips;

	private float vol;

	private List<AudioSource> audioSourceList;

	public AudioSource bgm_background;
	public AudioSource bgm_main;

	public AudioClip currMainBGMClip;
	
	public enum SFX_TYPE
	{
		CLAP_LOOP = 0,
		CLAP_FINAL,
		FALL_1,
		FALL_2
	};

	public enum BGM_TYPE
	{
		TITLE = 0,
		RADIO_BACKGROUND,
		RADIO_MUSIC_1,
		RADIO_MUSIC_2,
		RADIO_NEWS_1
	};
	

	private static AudioMgr _instance = null;

	public static AudioMgr Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<AudioMgr>() as AudioMgr;
			}

			return _instance;
		}
	}

	
	public void Start () {
		Init();
	}

	private void Init()
	{
		vol = 1.0f;

		audioSourceList = new List<AudioSource>();

		UnityEngine.Random.seed = (int)System.DateTime.Now.Ticks;
		
		for(int i = 0; i < sfxClips.Length; i ++)
		{
			GameObject go = new GameObject("audiosource_single"); 
			
			go.transform.parent = transform;
			
			AudioSource audioSource = go.AddComponent<AudioSource>(); 
			
			audioSource.clip =  sfxClips[i];
			
			audioSourceList.Add(audioSource); 		
		}
	}

	public void SetMianBGMLoop(bool _loop)
	{
		bgm_main.loop = _loop;
	}

	public void SwitchMainBGM(BGM_TYPE type)
	{
		AudioClip _bgm = bgmClips[(int)type];

		bgm_main.clip = _bgm;
		bgm_main.Play();

		currMainBGMClip = _bgm;
	}

	public void StopMainBGM()
	{
		bgm_main.Stop();

		currMainBGMClip = null;
	}

	public void SwitchBackgroundBGM(BGM_TYPE type)
	{
		AudioClip _bgm = bgmClips[(int)type];
		
		bgm_background.clip = _bgm;
		bgm_background.Play();
	}

	public void StopBackgroundBGM()
	{
		bgm_background.Stop();
	}
	
	public void PlaySingle(SFX_TYPE type, float volume)
	{ 
		AudioSource audioSource = audioSourceList[(int)type];
		
		audioSource.volume = volume * vol;
		
		audioSource.Play();
	}

	public bool IsMainBGMPlyaing()
	{
		return bgm_main.isPlaying;
	}

	public void PlayClip(SFX_TYPE type, float volume)
	{
		audio.volume = volume * vol;
		
		audio.PlayOneShot(sfxClips[(int)type], volume * vol) ;
	}
}

