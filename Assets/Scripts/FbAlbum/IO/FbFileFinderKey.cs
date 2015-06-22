using UnityEngine;
using System;

 
public class FbFileFinderKey : MonoBehaviour {
 	public FileFinder fileFinder;
	public LoadTexture loadTexture;
	public MyInteractiveConsole interactiveConsole;
	
	void Update() {
		if(Input.GetKeyUp(KeyCode.D)) {
		//	interactiveConsole.CallFBInit();
		}else if(Input.GetKeyUp(KeyCode.F)) {
		//	interactiveConsole.CallFBLogin();
			StartCoroutine(interactiveConsole.InitFacebook());
		}else if(Input.GetKeyUp(KeyCode.G)) {
			loadTexture.LoadFromFacebook();
		} else if(Input.GetKeyUp(KeyCode.H)) {
			interactiveConsole.GetFbFriend();
		}
	}
}