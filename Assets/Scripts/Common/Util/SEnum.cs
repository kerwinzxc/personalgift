using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;

//SEnum == String Enum
public class SEnum {
	public static Dictionary<Enum,StringValueAttribute> _stringValues = new Dictionary<Enum,StringValueAttribute> ();

	public static string GetStringValue(Enum value)
	{
		string output = null;
		Type type = value.GetType();
		
		//Check first in our cached results...
		if (_stringValues.ContainsKey(value))
			output = (_stringValues[value] as StringValueAttribute).Value;
		else 
		{
			//Look for our 'StringValueAttribute' 
			//in the field's custom attributes
			FieldInfo fi = type.GetField(value.ToString());
			StringValueAttribute[] attrs = 
				fi.GetCustomAttributes(typeof (StringValueAttribute), 
				                       false) as StringValueAttribute[];
			if (attrs.Length > 0)
			{
				_stringValues.Add(value, attrs[0]);
				output = attrs[0].Value;
			}
		}
		
		return output;
	}
}
