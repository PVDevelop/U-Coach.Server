using NUnit.Framework;
using System;

namespace TestNUnit
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class IntegrationAttribute : CategoryAttribute
    {
        public IntegrationAttribute() : base("Integration") { }
    }
}
