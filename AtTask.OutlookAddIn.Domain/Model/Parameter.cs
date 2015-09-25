using System.Collections.Generic;

namespace AtTask.OutlookAddIn.Domain.Model
{
    public class Parameter : NamedEntityBase
    {
        public ParameterDataType? DataType { get; set; }

        public ParameterDisplayType? DisplayType { get; set; }

        public List<ParameterOption> ParameterOptions { get; set; }

        public int? DisplaySize { get; set; }

        public string Description { get; set; }

        public override string GetObjectType() { return "parameter"; }
    }
}