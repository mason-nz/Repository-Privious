using System;
using System.Reflection;

namespace XYAuto.BUOC.ChiTuData2017.TaskAPI.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}