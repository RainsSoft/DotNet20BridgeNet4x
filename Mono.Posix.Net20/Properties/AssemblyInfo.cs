﻿//using System.Reflection;
//using System.Runtime.CompilerServices;
//using System.Runtime.InteropServices;

//// 有关程序集的一般信息由以下
//// 控制。更改这些特性值可修改
//// 与程序集关联的信息。
//[assembly: AssemblyTitle("Mono.Posix.Net20")]
//[assembly: AssemblyDescription("")]
//[assembly: AssemblyConfiguration("")]
//[assembly: AssemblyCompany("")]
//[assembly: AssemblyProduct("Mono.Posix.Net20")]
//[assembly: AssemblyCopyright("Copyright ©  2022")]
//[assembly: AssemblyTrademark("")]
//[assembly: AssemblyCulture("")]

//// 将 ComVisible 设置为 false 会使此程序集中的类型
////对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型
////请将此类型的 ComVisible 特性设置为 true。
//[assembly: ComVisible(false)]

//// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
//[assembly: Guid("a6afe267-5e33-44c0-a1b9-3283b95792a1")]

//// 程序集的版本信息由下列四个值组成: 
////
////      主版本
////      次版本
////      生成号
////      修订号
////
////可以指定所有这些值，也可以使用“生成号”和“修订号”的默认值
////通过使用 "*"，如下所示:
//// [assembly: AssemblyVersion("1.0.*")]
//[assembly: AssemblyVersion("1.0.0.0")]
//[assembly: AssemblyFileVersion("1.0.0.0")]



//
// AssemblyInfo.cs
//
// Author:
//   Andreas Nahr (ClassDevelopment@A-SoftTech.com)
//
// (C) 2003 Ximian, Inc.  http://www.ximian.com
// (C) 2004 Novell (http://www.novell.com)
//

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
//

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

[assembly: AssemblyVersion(Consts.FxVersion)]

[assembly: AssemblyTitle("System.Mono.Posix.Net20.dll")]
[assembly: AssemblyDescription("Unix Integration Classes")]

[assembly: AssemblyCompany(Consts.MonoCompany)]
[assembly: AssemblyProduct(Consts.MonoProduct)]
[assembly: AssemblyCopyright(Consts.MonoCopyright)]

[assembly: CLSCompliant(true)]
[assembly: ComVisible(false)]

/* TODO COMPLETE INFORMATION

[assembly: AssemblyFileVersion ("0.0.0.1")]

*/

[assembly: AssemblyDelaySign(true)]
[assembly: AssemblyKeyFile("../mono.pub")]

/*
 * TODO:
 * 
 * Anything implementing IDisposable should derive from MarshalByRefObject.
 * This is for remoting situations (e.g. across AppDomains).
 * Impacts UnixClient, UnixListener.
 * 
 * UnixPath.InvalidPathChars should be const, not readonly.
 * 
 * Mono.Remoting.Channels.Unix.UnixChannel.CreateMessageSink should have a LinkDemand
 * idential to IChannelSender's CreateMessageSink LinkDemand.
 * Repeat for all other members of UnixChannel, UnixClient, UnixServer.
 * 
 * Override .Equals and the == operator for all structures.
 */

