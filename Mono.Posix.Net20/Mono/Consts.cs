//
// Consts.cs
//
// Author:
//   Marek Sieradzki (marek.sieradzki@gmail.com)
//
// (C) 2006 Marek Sieradzki
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.IO;
using Microsoft.Build.Utilities;
public
#if NET_2_0
	static
#else
	sealed
#endif
	class Consts
{
#if !NET_2_0
	private Consts ()
	{
	}
#endif

	//
	// Use these assembly version constants to make code more maintainable.
	//

	public const string MonoVersion = "@MONO_VERSION@";
	public const string MonoCompany = "MONO development team";
	//public const string MonoProduct = "MONO Common language infrastructure";
	//public const string MonoCompany = "MONO & https://www.nuget.org/profiles/rains";
	public const string MonoProduct = "MONO & github.com/RainsSoft | nuget.org/profiles/rains";
	public const string MonoCopyright = "(c) various MONO Authors";

#if NET_4_0 || BOOTSTRAP_NET_4_0
	public const string FxVersion = "4.0.0.0";
	public const string FxFileVersion = "4.0.20506.1";
	
	public const string VsVersion = "0.0.0.0"; // Useless ?
	public const string VsFileVersion = "10.0.0.0"; // TODO:
#elif NET_3_5 || UNITY_3_5
	// Versions of .NET Framework 3.5 RTM
	public const string FxVersion = "3.5.0.0";
	public const string FxFileVersion = "3.5.21022.8";
	
	public const string VsVersion = "0.0.0.0"; // Useless ?
#elif NET_3_0 || UNITY_3_0
	public const string FxVersion = "3.0.0.0";
	public const string VsVersion = "8.0.0.0";
	public const string FxFileVersion = "3.0.4506.648";
	public const string VsFileVersion = "6.0.6001.17014";
#elif (NET_2_1 && !UNITY) || UNITY_2_1
	// Versions of .NET Framework for Silverlight 3.0
	public const string FxVersion = "2.0.5.0";
	public const string VsVersion = "9.0.0.0"; // unused, but needed for compilation
	public const string FxFileVersion = "3.0.40818.0";
	public const string VsFileVersion = "9.0.50727.42"; // unused, but needed for compilation
#elif NET_2_0 || BOOTSTRAP_NET_2_0
	// Versions of .NET Framework 2.0 RTM
	public const string FxVersion = "2.0.0.0";
	public const string VsVersion = "8.0.0.0";
	public const string FxFileVersion = "2.0.50727.1433";
	public const string VsFileVersion = "8.0.50727.1433";
#elif NET_1_1
	// Versions of .NET Framework 1.1 SP1
	public const string FxVersion = "1.0.5000.0";
	public const string VsVersion = "7.0.5000.0";
	public const string FxFileVersion = "1.1.4322.2032";
	public const string VsFileVersion = "7.10.6001.4";
#elif NET_1_0
	// Versions of .NET Framework 1.0 SP3
	public const string FxVersion = "1.0.3300.0";
	public const string VsVersion = "7.0.3300.0";
	public const string FxFileVersion = "1.0.3705.6018";
	public const string VsFileVersion = "7.0.9951.0";
#else
#error No profile symbols defined.
#endif

	//
	// Use these assembly name constants to make code more maintainable.
	//

	public const string AssemblyI18N = "I18N, Version=" + FxVersion + ", Culture=neutral, PublicKeyToken=0738eb9f132ed756";
	public const string AssemblyMicrosoft_VisualStudio = "Microsoft.VisualStudio, Version=" + FxVersion + ", Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
#if NET_2_0
	public const string AssemblyMicrosoft_VisualStudio_Web = "Microsoft.VisualStudio.Web, Version=" + VsVersion + ", Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
#endif
	public const string AssemblyMicrosoft_VSDesigner = "Microsoft.VSDesigner, Version=" + VsVersion + ", Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
	public const string AssemblyMono_Http = "Mono.Http, Version=" + FxVersion + ", Culture=neutral, PublicKeyToken=0738eb9f132ed756";
	public const string AssemblyMono_Posix = "Mono.Posix, Version=" + FxVersion + ", Culture=neutral, PublicKeyToken=0738eb9f132ed756";
	public const string AssemblyMono_Security = "Mono.Security, Version=" + FxVersion + ", Culture=neutral, PublicKeyToken=0738eb9f132ed756";
	public const string AssemblyMono_Messaging_RabbitMQ = "Mono.Messaging.RabbitMQ, Version=" + FxVersion + ", Culture=neutral, PublicKeyToken=0738eb9f132ed756";
	public const string AssemblyCorlib = "mscorlib, Version=" + FxVersion + ", Culture=neutral, PublicKeyToken=b77a5c561934e089";
	public const string AssemblySystem = "System, Version=" + FxVersion + ", Culture=neutral, PublicKeyToken=b77a5c561934e089";
	public const string AssemblySystem_Data = "System.Data, Version=" + FxVersion + ", Culture=neutral, PublicKeyToken=b77a5c561934e089";
	public const string AssemblySystem_Design = "System.Design, Version=" + FxVersion + ", Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
	public const string AssemblySystem_DirectoryServices = "System.DirectoryServices, Version=" + FxVersion + ", Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
	public const string AssemblySystem_Drawing = "System.Drawing, Version=" + FxVersion + ", Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
	public const string AssemblySystem_Drawing_Design = "System.Drawing.Design, Version=" + FxVersion + ", Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
	public const string AssemblySystem_Messaging = "System.Messaging, Version=" + FxVersion + ", Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
	public const string AssemblySystem_Security = "System.Security, Version=" + FxVersion + ", Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
	public const string AssemblySystem_ServiceProcess = "System.ServiceProcess, Version=" + FxVersion + ", Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
	public const string AssemblySystem_Web = "System.Web, Version=" + FxVersion + ", Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
	public const string AssemblySystem_Windows_Forms = "System.Windows.Forms, Version=" + FxVersion + ", Culture=neutral, PublicKeyToken=b77a5c561934e089";
#if NET_4_0 || BOOTSTRAP_NET_4_0
	public const string AssemblySystemCore_3_5 = "System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
	public const string AssemblySystem_Core = "System.Core, Version=" + FxVersion + ", Culture=neutral, PublicKeyToken=b77a5c561934e089";
#elif NET_2_1
	public const string AssemblySystem_Core = "System.Core, Version=" + FxVersion + ", Culture=neutral, PublicKeyToken=b77a5c561934e089";
#elif NET_2_0
	public const string AssemblySystem_Core = "System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
#endif
}
//public static class Consts {

//	public static bool RunningOnMono ()
//	{
//		return Type.GetType ("Mono.Runtime") != null;
//	}

//	public static string BinPath {
//		get {
//			if (RunningOnMono ()) {
//#if XBUILD_12
//				string profile = "xbuild_12";
//#elif NET_4_5
//				string profile = "net_4_5";
//#elif NET_4_0
//				string profile = "net_4_0";
//#elif NET_3_5
//				string profile = "net_3_5";
//#else
//				string profile = "net_2_0";
//#endif
//				var corlib = typeof (object).Assembly.Location;
//				var lib = Path.GetDirectoryName (Path.GetDirectoryName (corlib));
//				return Path.Combine (lib, profile);
//			} else {
//#if XBUILD_12
//				return ToolLocationHelper.GetPathToBuildTools ("12.0");
//#elif NET_4_5
//				return ToolLocationHelper.GetPathToDotNetFramework (TargetDotNetFrameworkVersion.Version45);
//#elif NET_4_0
//				return ToolLocationHelper.GetPathToDotNetFramework (TargetDotNetFrameworkVersion.Version40);
//#elif NET_3_5
//				return ToolLocationHelper.GetPathToDotNetFramework (TargetDotNetFrameworkVersion.Version35);
//#else
//				return ToolLocationHelper.GetPathToDotNetFramework (TargetDotNetFrameworkVersion.Version20);
//#endif
//			}
//		}
//	}

//	public static string ToolsVersionString {
//		get {
//#if XBUILD_12
//			return " ToolsVersion='12.0'";
//#elif NET_4_0
//			return " ToolsVersion='4.0'";
//#elif NET_3_5
//			return " ToolsVersion='3.5'";
//#else
//			return String.Empty;
//#endif
//		}
//	}

//	public static string GetTasksAsmPath ()
//	{
//#if XBUILD_12
//		return Path.Combine (BinPath, "Microsoft.Build.Tasks.v12.0.dll");
//#elif NET_4_0
//		return Path.Combine (BinPath, "Microsoft.Build.Tasks.v4.0.dll");
//#elif NET_3_5
//		return Path.Combine (BinPath, "Microsoft.Build.Tasks.v3.5.dll");
//#else
//		return Path.Combine (BinPath, "Microsoft.Build.Tasks.dll");
//#endif
//	}
//}
