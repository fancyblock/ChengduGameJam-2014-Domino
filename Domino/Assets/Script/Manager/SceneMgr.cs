using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneMgr : MonoBehaviour {

	private static SceneMgr _instance = null;

	public static SceneMgr Instance
	{
		get{

			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<SceneMgr>() as SceneMgr;
			}

			return _instance;
		}
	}

	private Stack<Scene> sceneStack;

	private Scene currScene;

	
	public Scene GetCurrScene()
	{
		return currScene;
	}

	void Awake()
	{
		sceneStack = new Stack<Scene>();
	}

	void Start()
	{
		PushScene(TitleScene.Instance);
	}

	public void PopScene()
	{
		if(sceneStack.Count > 0)
		{
			Scene oldTop = sceneStack.Peek();

			sceneStack.Pop();

			Scene newTop = sceneStack.Peek();

			if (newTop != null)
			{
				oldTop.OnStateOut(newTop);

				newTop.OnStateIn(oldTop);

				currScene = newTop;
			}
			else
			{	
				oldTop.OnStateOut(null);

				currScene = null;
			}


		}
	}

	public void PushScene(Scene scene)
	{
		if (sceneStack.Count == 0)
		{
			sceneStack.Push(scene);
			scene.OnStateIn(null);
		}
		else
		{
			Scene top = sceneStack.Peek();

			top.OnStateOut(scene);

			sceneStack.Push(scene);

			scene.OnStateIn(top);
		}

		currScene = scene;
	}
}
