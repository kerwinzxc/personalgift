using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Object = UnityEngine.Object;

public class Singleton<T> where T : UnityEngine.Object {
	private static T sInstance;
	
	public static void ClearInstance() {
		sInstance = null;
	}
	
	public static T instance {
		get {
			if(sInstance == null) {
				sInstance = (T)Object.FindObjectOfType(typeof(T));
			}
			return sInstance;
		}
		
		set {
			sInstance = value;
		}
	}

	public static T instanceNoServer {
		get {
			if(sInstance == null) {
				sInstance = (T)Object.FindObjectOfType(typeof(T));
				#if SINGLETON_PRINTS
				if (sInstance == null)
				{
					GeneratedPlugin.DebugLog("Could not find " + typeof(T).Name);
				}
				#endif
			}
			return sInstance;
		}
	}
	
	public static T instanceNoFind {
		get {
			if(sInstance == null) {
				#if SINGLETON_PRINTS
				GeneratedPlugin.DebugLog("Could not find " + typeof(T).Name);
				#endif
			}
			return sInstance;
		}
		
		set {
			sInstance = value;
		}
	}
	
	public static T instanceManagement(string assetPath)
	{
		#if UNITY_EDITOR
		if (sInstance == null)
		{
			sInstance = (T)Object.FindObjectOfType(typeof(T));
			if (sInstance == null)
			{
				if (Application.isPlaying)
				{
					EditorUtility.DisplayDialog("Error", "You must put a " + assetPath + " in your scene", "OK");
					UnityEditor.EditorApplication.ExecuteMenuItem("Edit/Play");
				}
				
				GameObject o = (GameObject)UnityEditor.AssetDatabase.LoadMainAssetAtPath(assetPath);
				if (o == null)
				{
					Debug.LogError("expecting " + assetPath + " to exist");
				}
				
				UnityEditor.PrefabUtility.InstantiatePrefab(o);
				
				sInstance = (T)Object.FindObjectOfType(typeof(T));
				
				if (sInstance == null)
				{
					Debug.LogError("expecting to have created an instance of " + assetPath);
				}
			}
		}
		return sInstance;
		#else
		if (sInstance == null)
		{
			//FIXME //TODO : manjeet : next line giving error on device so commented out
			//	sInstance = (T) Object.FindObjectsOfType(typeof(T));
		
			
			if (sInstance == null)
			{
				Debug.Log("You need to have an instance of " + typeof(T).Name + " in your scene");
				//GeneratedPlugin.DebugLogError("You need to have an instance of " + typeof(T).Name + " in your scene");
			}
		}
		return sInstance;
		#endif
	}
	
	public static T instanceTodoNull {
		get {
			return null;
		}
	}
}
