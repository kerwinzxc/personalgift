using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;

public class StringValueAttribute : System.Attribute
{
	
	private string _value;
	
	public StringValueAttribute(string value)
	{
		_value = value;
	}
	
	public string Value
	{
		get { return _value; }
	}
	
}


public class ForeignKeyAttribute : System.Attribute
{	
}


public class DbTableAttribute : System.Attribute
{
}
