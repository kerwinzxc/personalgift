using UnityEngine;
using System.Collections;

public class TakeScreenshot : MonoBehaviour {
	public bool takeScreenShot = false;
	public bool useUnityDefaultCapture = true;
	public int resWidth = 2550; 
	public int resHeight = 3300;

	private bool takingScreenShot = false;

	public static string ScreenShotName(int width, int height) {
		return string.Format("../../screen_{1}x{2}_{3}.png", 
		                     Application.dataPath, 
		                     width, height, 
		                     System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
	}
	
	public void TakeHiResShot() {
		takeScreenShot = true;
	}
	
	void LateUpdate() {
		takeScreenShot |=  (Input.GetKeyDown("k") );
		if (takeScreenShot) {
			StartCoroutine(TakeScreenShotCoroutine());
			takeScreenShot = false;
		}
	}

	public IEnumerator TakeScreenShotCoroutine() {
		while (takingScreenShot)
						yield return 0;
		takingScreenShot = true;
		if(useUnityDefaultCapture)
			UnityDefaultCapture();
		else
			CustomCaputre();
		takingScreenShot = false;
	}

	private void UnityDefaultCapture() {
		string filename = ScreenShotName(resWidth, resHeight);
		int superSize = Mathf.Max (1, resWidth / 512); //Factor by which to increase resolution.
		Application.CaptureScreenshot(filename , superSize);
		Debug.Log(string.Format("UnityDefaultCapture: Took screenshot to: {0}", filename));
	}

	private void CustomCaputre() {
		RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
		GetComponent<Camera>().targetTexture = rt;
		Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
		GetComponent<Camera>().Render();
		RenderTexture.active = rt;
		screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
		GetComponent<Camera>().targetTexture = null;
		RenderTexture.active = null; // JC: added to avoid errors
		Destroy(rt);
		byte[] bytes = screenShot.EncodeToPNG();
		string filename = ScreenShotName(resWidth, resHeight);
		System.IO.File.WriteAllBytes(filename, bytes);
		Debug.Log(string.Format("CustomCapture: Took screenshot to: {0}", filename));
	}

}