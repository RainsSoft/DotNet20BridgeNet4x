


#if NET_4_0

using System;

namespace System.Diagnostics.Contracts
{
	[ConditionalAttribute("CONTRACTS_FULL")]
	[AttributeUsageAttribute (AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.Delegate)]
	internal sealed class PureAttribute : Attribute
	{
	}
}

#endif
