using System;
using System.Collections.Generic;
using System.Text;

namespace System.Security
{
	//public delegate TResult Func<T, TResult>(T arg);
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public class MonoTODOAttribute : Attribute
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
