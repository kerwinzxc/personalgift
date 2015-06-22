using UnityEngine;
using System;
using System.Collections;

 
public class FileFinderMouse : MonoBehaviour {
 	public FileFinder fileFinder;
 	public LoadTexture loadTexture;
	void LateUpdate() {
		if( Input.GetMouseButtonDown(0))
			OnMouseDown();
	}
	
	//TODO : NOT BEING CALLED WHY?
	void OnMouseDown() {
		Debug.Log("MouseDown in FileFinderMouse");
		if(!fileFinder.showBrowser)
			StartCoroutine(SelectScreen());
	}
	
	public IEnumerator SelectScreen() {
		yield return 0;
		int layerMask = 1 << LayerMask.NameToLayer("Screen");
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100, layerMask))
		{
			Debug.Log("MouseDown in FileFinderMouse2");
            print("Hit " + LayerMask.LayerToName(hit.transform.gameObject.layer));
			Debug.DrawLine(ray.origin, hit.point);
			loadTexture.screenObject = hit.transform.gameObject;
			fileFinder.showBrowser = true;
		}
		yield break;
	}
	
}