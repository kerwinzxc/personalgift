using UnityEngine;
using System;

 
public class FileFinder : MonoBehaviour {
 
	public string m_textPath;
 	public event Action newFileSelected;
	public string selectionPatten;
	public bool showBrowser=true;
	protected FileBrowser m_fileBrowser;
 
	[SerializeField]
	protected Texture2D	m_directoryImage,
						m_fileImage;
 
	void Start() {
		m_fileBrowser = new FileBrowser(
			new Rect(1, 1, 400, 700),
			"Choose Text File",
			FileSelectedCallback
		);
		m_fileBrowser.SelectionPattern = selectionPatten;
		m_fileBrowser.DirectoryImage = m_directoryImage;
		m_fileBrowser.FileImage = m_fileImage;
	}
	
	protected void OnGUI() {
		if(showBrowser)
			OpenFileBrowser();
	}
 
	private void OpenFileBrowser() {
		if (m_fileBrowser != null) {
			m_fileBrowser.OnGUI();
		}
	}
	
	bool isFileSelected(string path) {
		return (path!="" && path!=" " && path!="/" && path!="/ ");
	}
	
	protected void FileSelectedCallback(string path) {
		m_textPath = path;
		showBrowser=false;
		if(isFileSelected(path))
		newFileSelected();
	}
}