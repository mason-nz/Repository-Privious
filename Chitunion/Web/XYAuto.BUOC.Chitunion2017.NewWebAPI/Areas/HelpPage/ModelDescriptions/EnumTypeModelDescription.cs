using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XYAuto.BUOC.Chitunion2017.NewWebAPI.Areas.HelpPage.ModelDescriptions
{
    public class EnumTypeModelDescription : ModelDescription
    {
        public EnumTypeModelDescription()
        {
            Values = new Collection<EnumValueDescription>();
        }

        public Collection<EnumValueDescription> Values { get; private set; }
    }
}