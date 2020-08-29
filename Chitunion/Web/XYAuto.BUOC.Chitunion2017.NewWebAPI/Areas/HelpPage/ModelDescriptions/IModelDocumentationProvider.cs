using System;
using System.Reflection;

namespace XYAuto.BUOC.Chitunion2017.NewWebAPI.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}