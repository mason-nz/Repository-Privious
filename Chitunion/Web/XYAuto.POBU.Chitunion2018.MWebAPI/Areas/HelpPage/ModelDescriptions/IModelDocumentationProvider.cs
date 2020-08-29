using System;
using System.Reflection;

namespace XYAuto.POBU.Chitunion2018.MWebAPI.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}