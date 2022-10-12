using System;
using System.Collections.Generic;
using System.Text;

namespace Mono.Security
{
	internal sealed class Locale
	{

		private Locale() {
		}

		public static string GetText(string msg) {
			return msg;
		}

		public static string GetText(string fmt, params object[] args) {
			return String.Format(fmt, args);
		}
	}
}
