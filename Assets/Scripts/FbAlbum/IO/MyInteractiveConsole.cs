using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class MyInteractiveConsole : InteractiveConsole {

	private static List<object>                 friends         = null;
	private static Dictionary<string, string>   profile         = null;

	void Start() {
		StartCoroutine(InitFacebook());
	}
	
	/////////////////////////////////////// 

	public IEnumerator InitFacebook() {
		Debug.Log ("Facebook initilising");
		CallFBInit();
		status = "FB.Init() called with " + FB.AppId;
		while(!FB.IsInitialized) yield return 0;
		
		Debug.Log (status);
		CallFBLogin();
		status = "Login called";
		while(!FB.IsLoggedIn) yield return 0;
		
		Debug.Log (status);
		GetFbProfileInfo ();

	//	while(!FB.IsLoggedIn) yield return 0;
	//	GetFbProfileInfo ();
	}


	public void GetFbProfileInfo()
	{
		FbDebug.Log("Logged in. ID: " + FB.UserId);
		// Reqest player info and profile picture
		FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id,picture)", Facebook.HttpMethod.GET, APICallback);
		FB.API(Util.GetPictureURL("me", 1024, 1024), Facebook.HttpMethod.GET, MyPictureCallback);
	}
	
	void APICallback(FBResult result)
	{
		FbDebug.Log("APICallback");
		if (result.Error != null)
		{
			FbDebug.Error(result.Error);
			// Let's just try again
			FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id,picture)", Facebook.HttpMethod.GET, APICallback);
			return;
		}
		
		profile = Util.DeserializeJSONProfile(result.Text);
		GameStateManager.Username = profile["first_name"];
		friends = Util.DeserializeJSONFriends(result.Text);
	}
	
	void MyPictureCallback(FBResult result)
	{
		FbDebug.Log("MyPictureCallback");
		if (result.Error != null)
		{
			FbDebug.Error(result.Error);
			// Let's just try again
			FB.API(Util.GetPictureURL("me", 512, 512), Facebook.HttpMethod.GET, MyPictureCallback);
			return;
		}
		GameStateManager.UserTexture = result.Texture;
	}
	
	public void GetFbFriend() {
		if (friends != null && friends.Count > 0)
		{
			// Select a random friend and get their picture
			Dictionary<string, string> friend = Util.RandomFriend(friends);
			GameStateManager.FriendName = friend["first_name"];
			GameStateManager.FriendID = friend["id"];
			FB.API(Util.GetPictureURL((string)friend["id"], 512, 512), Facebook.HttpMethod.GET, Util.FriendPictureCallback);
		}
	}
	
	public string[] GetAllFbFriendsURL(){
		List<string> picurls = new List<string>();
		if (friends != null && friends.Count > 0)
		{
			foreach (object s in friends) {
				var friend = ((Dictionary<string, object>)(s));
				Debug.Log(friend.ToString());
				Debug.Log("frind id "+(string)friend["id"] );
				picurls.Add( Util.DeserializePictureURLObject(friend["picture"] ) );
				//picurls.Add(Util.GetPictureURL((string)friend["id"], 512, 512));
			}
			//FB.API(Util.GetPictureURL((string)friend["id"], 512, 512), Facebook.HttpMethod.GET, Util.FriendPictureCallback);
		}
		return picurls.ToArray ();
	}

	void OnGUI()
	{
	}
}