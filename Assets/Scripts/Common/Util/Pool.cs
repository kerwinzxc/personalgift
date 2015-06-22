using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Pool
{
	
	public class ObjectPool
	{
		
		public const string ObjectPoolName = "ObjectPool";
		internal bool debugPooler = false;
		UnityEngine.Object baseObject; 
		public List<GameObject> objects; 
		internal int activecount;
		
		public ObjectPool (UnityEngine.Object baseObject)
		{
			this.baseObject = baseObject;
			objects = new List<GameObject> ();
		}
		
		//Fix
		public void AddPrebakedObject (UnityEngine.Object obj)
		{
			if (obj != null) {
				objects.Add ((GameObject)obj);
				((GameObject)obj).transform.parent = root;
			}
		}
		
		public Transform Instantiate (UnityEngine.Object baseObject, Vector3 position, Quaternion rotation)
		{
			
			// get the first unused object from the pool
			if (activecount < objects.Count) {
				GameObject o = objects [activecount];
				activecount++;
				
				if (o == null) {
					o = objects [activecount - 1] = SpawnNewObject (baseObject, position, rotation, activecount.ToString ()).gameObject;
					Debug.LogError ("object in pool is null " + baseObject.name + " spawning a new one");
				}
				
				//Rigidbody rb = o.GetComponent<Rigidbody>();
				//if(rb != null) rb.detectCollisions = true;
				
				Transform t = o.transform;
				t.position = position;
				t.rotation = rotation;
				
				o.SetActive (true);
				
				if (root != null)
					t.parent = root;
				return t;
			} else {
				Transform t = SpawnNewObject (baseObject, position, rotation, (objects.Count + 1).ToString ());
				objects.Add (t.gameObject);
				activecount++;
				return t;
			}
		}
		
		static Transform SpawnNewObject (UnityEngine.Object baseObject, Vector3 position, Quaternion rotation, string NameAppendix)
		{
			Transform t = null;
			if (baseObject.GetType () == typeof(Transform)) {
				t = (Transform)GameObject.Instantiate (baseObject, position, rotation);
			} else {
				GameObject obj = (GameObject)UnityEngine.Object.Instantiate (baseObject, position, rotation);
				t = obj.transform;
			}
			//AddPoolMaintenance( t, duration ); // check if t is an effect that will self-destruct, catch it and make it Pool.Destroy instead
			if (root != null)
				t.parent = root;
			
			t.gameObject.name += NameAppendix;
			
			return t;
		}
		
		void AddPoolMaintenance (Transform t, float timeout)
		{

		}
		
		public bool Destroy (GameObject obj)
		{
			if (obj == null)
				return false;
			bool destroyed = false;
			//Debug.Log("ObjectPool.Destroy(obj)");
			// move it to the back of the array
			for (int i=0; i<activecount; i++) {
				if (objects [i] == obj) {
					activecount--;
					if (activecount > 0) {
						objects [i] = objects [activecount];
						objects [activecount] = obj;
					}
					destroyed = true;
					//Debug.Log("ObjectPool.Destroy(obj) " + obj);
				}
			}
			
			if (destroyed) {
				// deactivate object
				//Debug.Log(obj.name + " calling SetActive(false) and crossing fingers...");
				
				//Rigidbody rb = obj.GetComponent<Rigidbody>();
				//if(rb != null) rb.detectCollisions = false;
				//Debug.Log("ObjectPool STARTING to deactivate " + obj.name + " itself.");
				
				obj.SetActive (false);
				
				//Debug.Log("FINISHED deactivating " + obj.name );
				
				if (Pool.root != null)
					obj.transform.parent = root;
				else
					obj.transform.parent = null;
			}
			
			return destroyed;
		}
		
		public void DestroyAll ()
		{
			if (objects == null)
				return;
			
			for (int i = 0; i < objects.Count; i++) {
				GameObject obj = objects [i];
				
				if (obj != null)
					obj.transform.parent = null;
			}
		}
		
		public void ClearAll ()
		{
			if (objects == null)
				return;
			
			for (int i = 0; i < objects.Count; i++) {
				GameObject obj = objects [i];
				
				if (obj == null) {
					Debug.LogError ("null object in pool " + baseObject.name);
					continue;
				}
				
				obj.SetActive (false);
				
				if (Pool.root != null) 
					obj.transform.parent = Pool.root;
				else
					obj.transform.parent = null;
			}
			
			activecount = 0;
		}
		
		// private bool debugBad = false;
		
		public void DebugCheckConsistency ()
		{
			/*
			// error only happens once
			if( debugBad ) return;
			
			for( int i=0; i<activecount; i++ ) {
				if( objects[i] == null ) {
					Debug.LogError( "Object " + i + " (" + prefab.name + ") is null.  This probably means it's been destroyed without using Pool.Destroy()." );
					debugBad = true;
				} else {
					if( !objects[i].activeSelf ) {
						// Debug.LogError( "Object " + i + " (" + prefab.name + ") is inactive despite being in the active section.  The pool is damaged." );
					}
				}
			}
			for( int i=activecount; i<objects.Count; i++ ) {
				if( objects[i] == null ) {
					Debug.LogError( "Object " + i + " (" + prefab.name + ") is null.  This probably means it's been destroyed without using Pool.Destroy()." );
					debugBad = true;
				} else {
					if( objects[i].activeSelf ) {
						Debug.LogError( "Object " + i + " (" + prefab.name + ") is active despite being in the inactive section.  The pool is damaged." );
					}
				}
			}
			*/
		}
		
		public static void DestroyMe ()
		{
			GameObject gameObject = GameObject.Find (ObjectPoolName);
			if (gameObject != null) {
				UnityEngine.Object.Destroy (gameObject);
			}
		}
	}
	
	public static event Action<Transform> objectInstantiated;
	
	static Dictionary<UnityEngine.Object,ObjectPool> pools;
	public static bool initialized = false;
	static string[] debugAlerts = new string[0];
	internal static Transform root = null;
	
	void Awake ()
	{
		initialized = false;
		debugAlerts = new string[0];
		root = null;
	}
	
	public static void Initialize ()
	{
		
		if (initialized == true) {
			return;
		}
		pools = new Dictionary<UnityEngine.Object,ObjectPool> ();
		
		if (root == null) {
			GameObject rootObj = GameObject.Find (ObjectPool.ObjectPoolName);
			
			if (rootObj == null)
				rootObj = new GameObject (ObjectPool.ObjectPoolName);	
			
			root = rootObj.transform;
			UnityEngine.Object.DontDestroyOnLoad (rootObj);
		}
		
		initialized = true;
	}

	public static Transform Instantiate (UnityEngine.Object baseObject)
	{
		return Instantiate (baseObject, Vector3.zero, Quaternion.identity);
	}
	
	public static Transform Instantiate (UnityEngine.Object baseObject, Vector3 position, Quaternion rotation)
	{
		return Instantiate (baseObject, position, rotation, 0);
	}
	
	public static Transform Instantiate (UnityEngine.Object baseObject, Vector3 position, Quaternion rotation, float timeout)
	{
		if (!initialized)
			Initialize ();
		
		if (baseObject == null) {
			Debug.LogError ("Attempting to instantiate null baseObject!");
			return null;
		}
		
		foreach (string alert in debugAlerts) {
			if (baseObject.name.Contains (alert)) {
				Debug.Log ("Instantiate: " + baseObject.name);
				break;
			}
		}
		
		// get the pool
		ObjectPool pool;
		if (!pools.TryGetValue (baseObject, out pool)) {
			pool = new ObjectPool (baseObject);
			pools [baseObject] = pool;
		}
		
		Transform newTransform = pool.Instantiate (baseObject, position, rotation);
		
		if (newTransform != null && root != null)
			newTransform.parent = root;
		
		if (objectInstantiated != null)
			objectInstantiated (newTransform);
		
		return newTransform;
	}
	
	public static void AddPrebakedObject (UnityEngine.Object objct, UnityEngine.Object obj)
	{
		if (obj == null || objct == null)
			return;
		
		if (!initialized) 
			Initialize ();
		
		ObjectPool pool;
		if (!pools.TryGetValue (objct, out pool)) {
			pool = new ObjectPool (objct);
			pools [objct] = pool;
		}
		
		pool.AddPrebakedObject (obj);
	}
	
	public static int GetPoolCount (UnityEngine.Object objctHint)
	{
		//Debug.Log("ObjectPool.Destroy(obj)");
		if (objctHint == null) {
			Debug.Log ("Pool.Destroy called with null objct key");
			return -1;
		}
		
		
		if (!pools.ContainsKey (objctHint)) {
			return -1;
		}
		
		return pools [objctHint].activecount;
	}
	
	public static void Destroy (GameObject obj, UnityEngine.Object baseObjectHint)
	{
		foreach (string alert in debugAlerts) {
			if (obj.name.Contains (alert)) {
				Debug.Log ("Destroy (with hint): " + obj.name);
				break;
			}
		}
		
		
		//Debug.Log("ObjectPool.Destroy(obj)");
		if (baseObjectHint == null) {
			Debug.Log ("Pool.Destroy called with null baseObject key");
			return;
		}
		
		
		if (!pools.ContainsKey (baseObjectHint)) {
			Debug.Log ("Pool.Destroy called with unknown baseObject key " + baseObjectHint.ToString ());
			return;
		}
		
		pools [baseObjectHint].Destroy (obj);
	}
	
	// no hint, linear search
	public static bool Destroy (GameObject obj)
	{
		if (obj == null)
			return false;
		
		foreach (string alert in debugAlerts) {
			if (obj.name.Contains (alert)) {
				Debug.Log ("Destroy: " + obj.name);
				break;
			}
		}
		
		if (!initialized) {
			Debug.Log ("can't pool destroy because pool not initialized");
			return false;
		}
		
		foreach (ObjectPool pool in pools.Values) {
			//Debug.Log("ObjectPool.Destroy(obj)");
			if (pool.Destroy (obj))
				return true;
		}
		
		// who cares?
		//Debug.Log("could not find active obj in any pools");
		return false;
	}
	
	public static void Destroy (Transform obj, Transform baseObjectHint)
	{
		foreach (string alert in debugAlerts) {
			if (obj.name.Contains (alert)) {
				Debug.Log ("Destroy (with hint): " + obj.name);
				break;
			}
		}
		
		
		//Debug.Log("ObjectPool.Destroy(obj)");
		if (baseObjectHint == null) {
			Debug.Log ("Pool.Destroy called with null baseObject key");
			return;
		}
		
		
		if (!pools.ContainsKey (baseObjectHint)) {
			Debug.Log ("Pool.Destroy called with unknown baseObject key " + baseObjectHint.name);
			return;
		}
		
		pools [baseObjectHint].Destroy (obj.gameObject);
	}
	
	// no hint, linear search
	public static bool Destroy (Transform obj)
	{
		if (obj == null)
			return false;
		
		foreach (string alert in debugAlerts) {
			if (obj.name.Contains (alert)) {
				Debug.Log ("Destroy: " + obj.name);
				break;
			}
		}
		
		if (!initialized) {
			Debug.Log ("can't pool destroy because pool not initialized");
			return false;
		}
		
		foreach (ObjectPool pool in pools.Values) {
			//Debug.Log("ObjectPool.Destroy(obj)");
			if (pool.Destroy (obj.gameObject))
				return true;
		}
		
		// who cares?
		//Debug.Log("could not find active obj in any pools");
		return false;
	}
	
	// no hint, linear search
	public static bool Contains (GameObject obj)
	{
		if (obj == null)
			return false;
		
		if (!initialized) {
			Debug.Log ("can't test for contains because pool not initialized");
			return false;
		}
		
		foreach (ObjectPool pool in pools.Values) {
			//Debug.Log("ObjectPool.Destroy(obj)");
			
			if (pool.objects.Contains (obj)) {
				
				int activeIndex = -1;
				for (int i = 0; i < pool.activecount; i++) {
					if (pool.objects [i] == obj) {
						activeIndex = i;
						break;
					}
				}
				
				Debug.Log ("baseObject pool contains object " + obj.name + ", activeIndex = " + activeIndex);
				
				return true;
			}
		}
		
		Debug.Log ("could not find obj '" + obj + "' in any pools");
		return false;
	}
	
	static void ClearOrDestroyAllPools (bool destroy)
	{
		if (pools == null)
			return;
		if (!initialized)
			return;
		
		IEnumerator<KeyValuePair<UnityEngine.Object, ObjectPool>> enumerator = pools.GetEnumerator ();
		
		while (enumerator.MoveNext()) {
			KeyValuePair<UnityEngine.Object, ObjectPool> val = enumerator.Current;
			
			ObjectPool pool = val.Value;
			
			if (pool != null) {
				if (destroy) {
					pool.DestroyAll ();
				} else {
					pool.ClearAll ();
				}
			}
		}
	}
	
	public static void ClearAllPools ()
	{
		ClearOrDestroyAllPools (false);
	}
	
	public static void DestroyAllPools ()
	{
		ClearOrDestroyAllPools (true);
		initialized = false;
	}
	
	public static void DebugCheckConsistency ()
	{
		// if an object has been destroyed outside of Pool.Destroy, it will show up as a null object in the array
		foreach (ObjectPool pool in pools.Values) {
			pool.DebugCheckConsistency ();
		}
	}
	
	public static Dictionary<UnityEngine.Object, ObjectPool> DebugGetPools ()
	{
		return pools;
	}
	
	public static void SetDebugAlerts (string[] alerts)
	{
		debugAlerts = alerts;
	}
}

