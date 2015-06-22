using UnityEngine;
using System;

 
public class TextFileFinder : MonoBehaviour {
 
	public static string m_textPath;
 	public static event Action newFileSelected;
	protected FileBrowser m_fileBrowser;
 
	[SerializeField]
	protected Texture2D	m_directoryImage,
						m_fileImage;
 
	protected void OnGUI () {
		if (m_fileBrowser != null) {
			m_fileBrowser.OnGUI();
		} else {
			OnGUIMain();
		}
	}
 
	protected void OnGUIMain() {
 
		GUILayout.BeginHorizontal();
			GUILayout.Label("Text File", GUILayout.Width(100));
			GUILayout.FlexibleSpace();
			GUILayout.Label(m_textPath ?? "none selected");
			if (GUILayout.Button("...", GUILayout.ExpandWidth(false))) {
				m_fileBrowser = new FileBrowser(
					new Rect(100, 100, 600, 500),
					"Choose Text File",
					FileSelectedCallback
				);
				m_fileBrowser.SelectionPattern = "*.jpg";
				m_fileBrowser.DirectoryImage = m_directoryImage;
				m_fileBrowser.FileImage = m_fileImage;
			}
		GUILayout.EndHorizontal();
	}
 
	protected void FileSelectedCallback(string path) {
		m_fileBrowser = null;
		m_textPath = path;
	}
}