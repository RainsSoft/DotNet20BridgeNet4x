using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	internal class MonoTODOAttribute : Attribute
	{

		string comment;

		public MonoTODOAttribute() {
		}

		public MonoTODOAttribute(string comment) {
			this.comment = comment;
		}

		public string Comment {
			get { return comment; }
		}
	}
}
