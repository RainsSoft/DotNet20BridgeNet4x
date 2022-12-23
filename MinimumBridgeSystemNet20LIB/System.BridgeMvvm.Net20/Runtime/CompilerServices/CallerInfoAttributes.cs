namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class CallerFilePathAttribute : Attribute
    {
        public CallerFilePathAttribute() { }
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class CallerLineNumberAttribute : Attribute
    {
        public CallerLineNumberAttribute() { }
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    internal sealed class CallerMemberNameAttribute : Attribute
    {
        public CallerMemberNameAttribute() { }
    }
}