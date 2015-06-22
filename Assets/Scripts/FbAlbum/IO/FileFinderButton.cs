using UnityEngine;
using System;

 
public class FileFinderButton : MonoBehaviour {
 	public FileFinder fileFinder;
	
	private string m_textPath;
 	public bool showBrowser=false;
	
	protected void OnGUI() {
		fileFinder.newFileSelected+=FileSelectedCallback;
		if(showBrowser)
			OnGUIMain();
	}
	
	protected void OnGUIMain() {
		GUILayout.BeginHorizontal();
			GUILayout.Label("Text File", GUILayout.Width(100));
			GUILayout.FlexibleSpace();
			GUILayout.Label(m_textPath ?? "none selected");
		if (GUILayout.Button("...", GUILayout.ExpandWidth(false))) {
			fileFinder.showBrowser = true;
		}
		GUILayout.EndHorizontal();
	}
 
	protected void FileSelectedCallback() {
		m_textPath = fileFinder.m_textPath;
		//showBrowser = true;
	}
}