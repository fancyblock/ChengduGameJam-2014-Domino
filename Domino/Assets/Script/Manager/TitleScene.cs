using UnityEngine;
using System.Collections;

public class TitleScene : Scene {

	private static TitleScene _instance;

	public static TitleScene Instance
	{
		get{

			Debug.Log("title scene get");

			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<TitleScene>() as TitleScene;

				Debug.Log("found _instance: " + _instance);
			}

			Debug.Log("_instance: " + _instance);
			
			return _instance;
		}
	}

	void Awake()
	{
		//gameObject.SetActive(false);
		_instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetMouseButtonUp(0))
		{
			SceneMgr.Instance.PushScene(GameScene.Instance);
		}
	}

	public override void OnStateIn(Scene last)
	{
		gameObject.SetActive(true);


		AudioMgr.Instance.SwitchMainBGM(AudioMgr.BGM_TYPE.TITLE);

	}

	public override void OnStateOut(Scene next)
	{
		if(next == GameScene.Instance)
		{
			gameObject.SetActive(false);

			AudioMgr.Instance.StopMainBGM();
		}
	}
}
