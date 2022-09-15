//using System.Reflection;
//using System.Runtime.CompilerServices;
//using System.Runtime.InteropServices;

//// 有关程序集的一般信息由以下
//// 控制。更改这些特性值可修改
//// 与程序集关联的信息。
//[assembly: AssemblyTitle("System.Core.Net20")]
//[assembly: AssemblyDescription("")]
//[assembly: AssemblyConfiguration("")]
//[assembly: AssemblyCompany("")]
//[assembly: AssemblyProduct("System.Core.Net20")]
//[assembly: AssemblyCopyright("Copyright ©  2022")]
//[assembly: AssemblyTrademark("")]
//[assembly: AssemblyCulture("")]

//// 将 ComVisible 设置为 false 会使此程序集中的类型
////对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型
////请将此类型的 ComVisible 特性设置为 true。
//[assembly: ComVisible(false)]

//// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
//[assembly: Guid("1e9498fc-1f70-4ae6-aae3-89043d4c995c")]

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
// Authors:
//	Marek Safar (marek.safar@gmail.com)
//
// Copyright (C) 2007-2008 Novell, Inc (http://www.novell.com)
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
using System.Resources;
using System.Security;
using System.Security.Permissions;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about the System.Core assembly

[assembly: AssemblyTitle("System.Core.Net20.dll")]
[assembly: AssemblyDescription("System.Core for Net2.0 and Unity")]
[assembly: AssemblyDefaultAlias("System.Core.Net20.dll")]

[assembly: InternalsVisibleTo("System.Threading.Net20")]

[assembly: AssemblyCompany(Consts.MonoCompany)]
[assembly: AssemblyProduct(Consts.MonoProduct)]
[assembly: AssemblyCopyright(Consts.MonoCopyright)]
[assembly: AssemblyVersion(Consts.FxVersion)]
[assembly: SatelliteContractVersion(Consts.FxVersion)]
[assembly: AssemblyInformationalVersion(Consts.FxFileVersion)]
[assembly: AssemblyFileVersion(Consts.FxFileVersion)]

[assembly: NeutralResourcesLanguage("en-US")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyDelaySign(true)]
#if NET_2_1
	// attributes specific to FX 3.5
	[assembly: AssemblyKeyFile ("../silverlight.pub")]
#else
// attributes specific to Silverlight 2.0
//[assembly: AssemblyKeyFile("../ecma.pub")]

//[assembly: AllowPartiallyTrustedCallers]
[assembly: DefaultDependency(LoadHint.Always)]
[assembly: SecurityCritical]
[assembly: StringFreezing]
#endif

[assembly: ComVisible(false)]

#if NET_4_0
[assembly: TypeForwardedTo (typeof (System.Security.Cryptography.Aes))]
[assembly: TypeForwardedTo (typeof (System.Threading.LazyThreadSafetyMode ))]
[assembly: TypeForwardedTo (typeof (System.Lazy<>))]
#endif
