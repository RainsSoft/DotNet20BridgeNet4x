
using System;

namespace Mono.Core
{
    /// <summary>
    /// When specified on a property or field, it will not be used when serializing/deserializing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public class DataMemberIgnoreAttribute : Attribute
    {
    }
}