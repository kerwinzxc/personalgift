using UnityEngine;
using System.Collections;

public class CoreBehaviour : MonoBehaviour
{
	[HideInInspector]
	public new Rigidbody rigidbody;
	[HideInInspector]
	public new Transform transform;
	[HideInInspector]
	public new Renderer renderer;
	[HideInInspector]
	public new Camera camera;
	
	//etc
	public virtual void Awake() {
		transform = gameObject.transform;
		if (gameObject.GetComponent<Rigidbody>()) rigidbody = gameObject.GetComponent<Rigidbody>(); //Overrides the slow rigidbody reference! (Thanks to Fabien Benoit-Koch for the tip)
		if (gameObject.GetComponent<Camera>()) camera = gameObject.GetComponent<Camera>();
		if (gameObject.GetComponent<Renderer>()) renderer = gameObject.GetComponent<Renderer>();
//		Debug.Log ("Caching complete.");
	}
}

