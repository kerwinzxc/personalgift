using UnityEngine;
using System.Collections;

namespace UnityExtensions
{
	//Must be in a static class 
	public static class GameObjectExtensions
	{
		public static bool HasRigidbody(this GameObject gobj)
		{
			return (gobj.GetComponent<Rigidbody>() != null);
		}
		
		public static bool HasAnimation(this GameObject gobj)
		{
			return (gobj.GetComponent<Animation>() != null);
		}

	}

}