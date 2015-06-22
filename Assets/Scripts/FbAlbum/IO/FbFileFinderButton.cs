using UnityEngine;
using System;

 
public class FbFileFinderButton : MonoBehaviour {
 	public FileFinder fileFinder;
	public LoadTexture loadTexture;
	public MyInteractiveConsole interactiveConsole;
	private string m_textPath;
 	public bool showBrowser=false;
	
	protected void OnGUI() {
		//fileFinder.newFileSelected+=FileSelectedCallback;
		if(showBrowser)
			OnGUIMain();
	}
	
	protected void OnGUIMain() {
		GUILayout.BeginHorizontal();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("fbinit", GUILayout.ExpandWidth(false))) {
			StartCoroutine(interactiveConsole.InitFacebook());
		}
		if (GUILayout.Button("f", GUILayout.ExpandWidth(false))) {
			interactiveConsole.GetFbFriend();
		}
		if (GUILayout.Button("fb", GUILayout.ExpandWidth(false))) {
			loadTexture.LoadFromFacebook();
		}
		GUILayout.EndHorizontal();
	}
 
	protected void FileSelectedCallback() {
		m_textPath = fileFinder.m_textPath;
		//showBrowser = true;
	}
}