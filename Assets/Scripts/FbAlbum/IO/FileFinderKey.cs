using UnityEngine;
using System;

 
public class FileFinderKey : MonoBehaviour {
 	public FileFinder fileFinder;
	
	void Update() {
		if(Input.GetKeyUp(KeyCode.A)) {
			fileFinder.showBrowser = true;
		}
	}
}