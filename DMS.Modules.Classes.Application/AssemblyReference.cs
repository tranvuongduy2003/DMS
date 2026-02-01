using System.Reflection;

namespace DMS.Modules.Classes.Application;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
