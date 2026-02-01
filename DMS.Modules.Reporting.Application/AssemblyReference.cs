using System.Reflection;

namespace DMS.Modules.Reporting.Application;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
