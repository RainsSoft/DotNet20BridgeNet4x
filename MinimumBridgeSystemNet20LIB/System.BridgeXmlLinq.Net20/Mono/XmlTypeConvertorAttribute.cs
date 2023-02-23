using System;
using System.Collections.Generic;
using System.Text;

namespace System.Xml.Serialization
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	internal class XmlTypeConvertorAttribute : Attribute
	{
		/*
		 * Bug #12571:
		 * 
		 * System.Xml.Linq.XElement should be deserializable from an XmlElement.
		 * 
		 * Types can now register a custom deserializer by adding this custom attribute.
		 * Method is the name of a private 'static method (static object)' method that will
		 * be invoked to construct an instance of the object.
		 * 
		 */
		public string Method {
			get;
			private set;
		}

		public XmlTypeConvertorAttribute(string method) {
			Method = method;
		}
	}
}
