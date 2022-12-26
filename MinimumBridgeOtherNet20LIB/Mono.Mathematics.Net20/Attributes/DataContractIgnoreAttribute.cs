
using System;
using System.Collections.Generic;



namespace Mono.Core
{
    [AttributeUsage(AttributeTargets.Delegate | AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Class, Inherited = false)]
    public class DataContractIgnoreAttribute : Attribute
    {
    }
}
