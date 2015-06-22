using UnityEngine;
using System.Collections;
using System;

public class LoadTexture : MonoBehaviour {
	public GameObject screenObject;
	
	public FileFinder fileFinder;
	public Material material;
	public Material loadingMaterial;
	public Texture2D defaultImage;
	public Texture2D loadingImage;
	// Use this for initialization
	void Start () {
		/*
		var go = new GameObject.CreatePrimitive(PrimitiveType.Plane); 
		*/
		//material = screenObject.renderer.material;
		//material.mainTexture = defaultImage;
		
		fileFinder.selectionPatten = "*.png";
		fileFinder.newFileSelected+=LoadByFileExplorer;

		//LoadFromResources();
		//LoadFromFile();
		LoadFromUrl();
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	public void LoadFromResources() {
		//string texture = "Assets/Resources/Textures/Turner.png";
		//Texture2D inputTexture = (Texture2D)Resources.LoadAssetAtPath(texture, typeof(Texture2D));
		screenObject.GetComponent<Renderer>().material.mainTexture = Resources.Load("Angel-47") as Texture;
		//material.SetTexture("_MainTex", texTmp);
	}
	
	public void LoadFromFacebook() {
		//string texture = "Assets/Resources/Textures/Turner.png";
		//Texture2D inputTexture = (Texture2D)Resources.LoadAssetAtPath(texture, typeof(Texture2D));
		screenObject.GetComponent<Renderer>().material.mainTexture = GameStateManager.FriendTexture;//UserTexture;
		//material.SetTexture("_MainTex", texTmp);
	}

	public void LoadFromFile() {
		StartCoroutine(CoroutineLoadFromUrl(@"file://C:\Users\msudhanshu\Desktop\2048win.png"));
	}
	
	public void LoadByFileExplorer() {
		StartCoroutine(CoroutineLoadFromUrl(@"file://"+fileFinder.m_textPath));
	}
	
	public void LoadFromUrl() {
		//http://www.hdwallpapers.in/walls/genelia_lovely-normal.jpg
		StartCoroutine(CoroutineLoadFromUrl(@"http://www.nilacharal.com/enter/celeb/images/Genelia.jpg"));
	}
	
	IEnumerator CoroutineLoadFromUrl(string url) {
		Material tmaterial = screenObject.GetComponent<Renderer>().material;
		Debug.Log("File being loaded: "+url);
			WWW www= new WWW (url);
		//todo : from cache;;;;;
			while(!www.isDone) {
				screenObject.GetComponent<Renderer>().material = loadingMaterial;
				//material.mainTexture = loadingImage;
				yield return 0;
			}
		    // Wait for download to complete
		  //  yield www;
		Texture2D texTmp = new Texture2D(1024, 1024, TextureFormat.DXT1, false);
        //LoadImageIntoTexture compresses JPGs by DXT1 and PNGs by DXT5    
		if(texTmp!=null) {
			try {
     	  	 www.LoadImageIntoTexture(texTmp);
				tmaterial.mainTexture = texTmp;//www.texture; 
			} catch (Exception e) {
				Debug.Log(e.ToString());
			}
		}
		screenObject.GetComponent<Renderer>().material = tmaterial;
		//material.SetTexture("_MainTex", texTmp);
			yield break;
	}
	
	
	/*public ArrayList imageBuffer = new ArrayList();
 
	private void LoadImages()
	{
	    //load all Texture2D files in Resources/Images folder
	    Object[] textures = Resources.LoadAll("Images");
	    for (int i=0; i < textures.Length; i++)
	        imageBuffer.Add(textures[i]);
	}
	*/
	
	
	/*
	function UploadPNG () {

    // We should only read the screen bufferafter rendering is complete

    yield WaitForEndOfFrame();

    

    // Create a texture the size of the screen, RGB24 format

    var width = Screen.width;

    var height = Screen.height;

    var tex = new Texture2D (width, height, TextureFormat.RGB24, false);

    // Read screen contents into the texture

    tex.ReadPixels (Rect(0, 0, width, height), 0, 0);

    tex.Apply ();

 

    // Encode texture into PNG

    var bytes = tex.EncodeToPNG();

    Destroy (tex);

 

    // For testing purposes, also write to a file in the project folder

    //File.WriteAllBytes(Application.dataPath + "/SavedScreen.png", bytes);

    File.WriteAllBytes("/sdcard/testfolder/testWrite.png", bytes);

}
*/
	
	
	
}
