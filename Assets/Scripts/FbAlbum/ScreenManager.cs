using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

namespace Album3D {

public class ScreenManager : Manager<ScreenManager> {

	public MyInteractiveConsole fbManager;
	public bool temploadflag = false;
	List<Screen> allScreens = new List<Screen> ();

		//CONSUMER
	SortedDictionary<int,Queue<Screen>> screenQueue = new SortedDictionary<int, Queue<Screen>> ();
		//producer
	Queue<string> imageURLS = new Queue<string>();

	/* Implement this function */
	override public void StartInit(){
			fbManager = GetComponent<MyInteractiveConsole> ();
			//AddImageURL (@"/Users/manjeet/Desktop/");
		}
	
	/* Implement this function */
	override public void PopulateDependencies(){}

	void LateUpdate() {
			if (Input.GetMouseButtonDown (0))
								temploadflag = true;
			if (temploadflag) {
				if(imageURLS.Count <=0) {
					AddFbFriendImageURL();
					//AddImageURL (@"/Users/manjeet/Desktop/");			
					//AddResourceImageURL(1);
				}
			    ResetAllScreen ();
			    temploadflag = false;
			}
			FillScreens ();
	}

	public void AddScreen(Screen screen, int priority = 3) {
		screen.SetPriority (priority);
		allScreens.Add (screen);
		AddInPriorityScreen (screen);
	}

	
	public void FillScreen(Screen screen, Texture texture) {
		screen.SetTexture (texture);
	}

	public void ResetAllScreen() {
			screenQueue.Clear ();
			foreach (Screen screen in allScreens) 
						AddInPriorityScreen (screen);
	}

	private void AddInPriorityScreen(Screen screen) {
			if (!screenQueue.ContainsKey (screen.GetPriority ())) {
				screenQueue.Add (screen.GetPriority (), new Queue<Screen> ());
			}
			screenQueue [screen.GetPriority ()].Enqueue (screen);
	}

	private Screen GetPriorityScreen ()
	{
			foreach (int priorityKey in screenQueue.Keys) {
				if (screenQueue [priorityKey].Count > 0) {
					return screenQueue [priorityKey].Dequeue ();
			}
		}
		return null;
	}
	
	public void AddImageURL(string dirPath) {
			string defaultUrl;		
			foreach (string filePath in Directory.GetFiles(dirPath)) {
//				Debug.Log("file added in imagequeue file://"+filePath);
				imageURLS.Enqueue(@"file://"+filePath);
			}
			defaultUrl = @"http://www.nilacharal.com/enter/celeb/images/Genelia.jpg";
		//	imageURLS.Enqueue(defaultUrl);
			defaultUrl = @"http://www.elisha-cuthbert.com/hairstyles/elisha-cuthbert-hairstyle-6.jpg";
		//	imageURLS.Enqueue(defaultUrl);
			defaultUrl =@"file:///Users/manjeet/Desktop/Screen Shot 2014-08-13 at 8.19.38 pm.png";
			imageURLS.Enqueue(defaultUrl);
	}


		//TODO : in case of resources .... dowload should be diffrent , not like www but resource.load()
		public void AddResourceImageURL(int albumNo) {
			string filePath ="Assets/Resources/Album3D/" ;		
	    	DirectoryInfo dir = new DirectoryInfo (filePath+albumNo);
			FileInfo[] info = dir.GetFiles ("*.png");
			Debug.Log (info.ToString ());
			info.Select (f => f.FullName).ToArray ();
			foreach (FileInfo f in info) { 
			//	Debug.Log(f.FullName);
				imageURLS.Enqueue(@"file://"+f.FullName);
			}
		}

		public void AddFbFriendImageURL() {
			Debug.Log ("manjeet : Getting fb freinds");
			string[] friendUrls = fbManager.GetAllFbFriendsURL();
			foreach (string url in friendUrls) { 
				Debug.Log(url);
				imageURLS.Enqueue(url);
			}
		}

	public void FillScreens() {
		if(imageURLS.Count > 0 ) {
			Screen screen = GetPriorityScreen ();
			if (screen != null) {
				string defaultUrl = imageURLS.Dequeue ();
				screen.ImageName = defaultUrl;
				StartCoroutine( CoroutineLoadFromUrl(screen,defaultUrl, new ImageDownloadDelegate(ImageDownloadCallback) ) );
			}
		}
	}

	public delegate void ImageDownloadDelegate (Screen screen, Texture texture);
	


	/// /////////
	void LoadFbImageAPI (Screen screen, string url, ImageDownloadDelegate callback)
	{
		FB.API(url,Facebook.HttpMethod.GET,result =>
		       {
			if (result.Error != null)
			{
				Util.LogError(result.Error);
				return;
			}
			
			var imageUrl = Util.DeserializePictureURLString(result.Text);
			StartCoroutine(CoroutineLoadFromUrl(screen,imageUrl,callback));
		});
	}
	void LoadPictureURL (Screen screen, string url, ImageDownloadDelegate callback){
			StartCoroutine(CoroutineLoadFromUrl(screen, url,callback));
	}
		//
		//FB.API(Util.GetPictureURL((string)friend["id"], 512, 512), Facebook.HttpMethod.GET, Util.FriendPictureCallback);
		/// ///////////////



	public void ImageDownloadCallback(Screen screen,Texture texture) {
			screen.SetTexture(texture);
	}
	

		//1,abstractooin at this level , for facebook dowloader api coroutine 
		//2. abstraction for downloading through , www, or resourceload, or facebookdownloadapi
	IEnumerator CoroutineLoadFromUrl(Screen screen, string url, ImageDownloadDelegate callback) {
		Debug.Log("File being loaded: "+url);
		screen.SetLoading();
		WWW www= new WWW (url);
		//todo : from cache;;;;;
		while(!www.isDone) {
			yield return 0;
		}
		// Wait for download to complete
		//  yield www;
		Texture2D texTmp = new Texture2D(1024, 1024, TextureFormat.DXT5, false); //LoadImageIntoTexture compresses JPGs by DXT1 and PNGs by DXT5    
		if(texTmp!=null) {
			try {

				www.LoadImageIntoTexture(texTmp);
				screen.SetLoading(false);
				callback(screen, www.texture);
			} catch (Exception e) {
				Debug.Log(e.ToString());
			}
		}
		yield break;
	}

}

}




/*
FileInfo[] fileInfo = levelDirectoryPath.GetFiles("*.*", SearchOption.AllDirectories);
 
foreach (FileInfo file in fileInfo) {
    // file name check
    if (file.Name == "something") {
        ...
    }
    // file extension check
    if (file.Extension == ".ext") {
        ...
    }
    // etc.
}
if (File.Exists("/path/to/file.ext")) {}

 * */