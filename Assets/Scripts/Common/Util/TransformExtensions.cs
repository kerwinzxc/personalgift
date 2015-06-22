using UnityEngine;
using System.Collections;

namespace UnityExtensions
{
	//Must be in a static class 
	public static class TransformExtensions
	{
		//Even though they are used like normal methods, extension
		//methods must be declared static. Notice that the first
		//parameter has the 'this' keyword followed by a Transform
		//variable. This variable denotes which class the extension
		//method becomes a part of.
		public static void ResetTransformation(this Transform trans)
		{
			trans.position = Vector3.zero;
			trans.localRotation = Quaternion.identity;
			trans.localScale = new Vector3(1, 1, 1);
		}

		//Function must be static
		//First parameter has "this" in front of type
		public static void SetPositionX(this Transform t, float newX)
		{
			t.position = new Vector3(newX, t.position.y, t.position.z);
		}

		public static void SetPositionY(this Transform t, float newY)
		{
			t.position = new Vector3(t.position.x, newY, t.position.z);
		}
		
		public static void SetPositionZ(this Transform t, float newZ)
		{
			t.position = new Vector3(t.position.x, t.position.y, newZ);
		}

		public static void SetLocalPositionX(this Transform t, float newX)
		{
			t.localPosition = new Vector3(newX, t.localPosition.y, t.localPosition.z);
		}
		
		public static void SetLocalPositionY(this Transform t, float newY)
		{
			t.localPosition = new Vector3(t.localPosition.x, newY, t.localPosition.z);
		}
		
		public static void SetLocalPositionZ(this Transform t, float newZ)
		{
			t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, newZ);
		}

		public static float GetPositionX(this Transform t)
		{
			return t.position.x;
		}
		
		public static float GetPositionY(this Transform t)
		{
			return t.position.y;
		}
		
		public static float GetPositionZ(this Transform t)
		{
			return t.position.z;
		}

	}

}