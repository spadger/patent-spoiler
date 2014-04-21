using System;

namespace PatentSpoiler.App.ServiceBinding
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class BindInRequestScopeAttribute : Attribute
    { }
}