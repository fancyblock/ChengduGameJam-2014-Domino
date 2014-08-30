using UnityEngine;
using System.Collections;

public class GameScene : Scene {

	private static GameScene _instance;
	
	public static GameScene Instance
	{
		get{
			
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GameScene>() as GameScene;
			}
			
			return _instance;
		}
	}

	private bool waitBGM = false;

	private int lastBGMIndex = 0;

	private bool firstBGM = true;

	// Use this for initialization
	void Start () {
	
	}


	void Awake()
	{
		_instance = this;

		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{

		if(Input.GetMouseButtonUp(0))
		{
			SceneMgr.Instance.PopScene();
		}


		if (!waitBGM)
		{
			StartCoroutine(TryToSwitchBGM());
		}
	}
	
	IEnumerator TryToSwitchBGM()
	{
		bool playing = AudioMgr.Instance.IsMainBGMPlyaing();
		
		if(!playing)
		{
			AudioMgr.BGM_TYPE type;

			int index;

			if(firstBGM)
			{
				type = AudioMgr.BGM_TYPE.RADIO_NEWS_1;

				firstBGM = false;

				index = (int)type;

			}
			else
			{

				int max = (int)AudioMgr.BGM_TYPE.RADIO_NEWS_1;
				int min = (int)AudioMgr.BGM_TYPE.RADIO_MUSIC_1;

				index = Random.Range(min, max);

				type = (AudioMgr.BGM_TYPE)index;

				if (lastBGMIndex != 0 && lastBGMIndex == index)
				{
					type = AudioMgr.BGM_TYPE.RADIO_NEWS_1;
				}
			}

			float waitTime = Random.Range(10.0f, 20.0f);

			waitBGM = true;

			yield return new WaitForSeconds(waitTime);

			Debug.Log("type : " + type);


			AudioMgr.Instance.SwitchMainBGM(type);
			
			lastBGMIndex = index;

			waitBGM = false;
		}
		else
		{
			return false;
		}
		
	}

	public override void OnStateIn(Scene last)
	{
		gameObject.SetActive(true);


		AudioMgr.Instance.SwitchBackgroundBGM(AudioMgr.BGM_TYPE.RADIO_BACKGROUND);

		AudioMgr.Instance.SetMianBGMLoop(false);

	}
	
	public override void OnStateOut(Scene next)
	{
		if(next == TitleScene.Instance)
		{
			gameObject.SetActive(false);

			AudioMgr.Instance.StopBackgroundBGM();

			AudioMgr.Instance.SetMianBGMLoop(true);
		}
	}
}
