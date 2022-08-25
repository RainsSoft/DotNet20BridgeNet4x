using System;
using System.Runtime.InteropServices;

namespace System {
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

	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public class MonoDocumentationNoteAttribute : MonoTODOAttribute
	{

		public MonoDocumentationNoteAttribute(string comment)
			: base(comment) {
		}
	}

	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public class MonoExtensionAttribute : MonoTODOAttribute
	{

		public MonoExtensionAttribute(string comment)
			: base(comment) {
		}
	}

	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public class MonoInternalNoteAttribute : MonoTODOAttribute
	{

		public MonoInternalNoteAttribute(string comment)
			: base(comment) {
		}
	}

	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public class MonoLimitationAttribute : MonoTODOAttribute
	{

		public MonoLimitationAttribute(string comment)
			: base(comment) {
		}
	}

	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public class MonoNotSupportedAttribute : MonoTODOAttribute
	{

		public MonoNotSupportedAttribute(string comment)
			: base(comment) {
		}
	}
}

