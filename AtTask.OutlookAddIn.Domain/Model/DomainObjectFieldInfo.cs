namespace AtTask.OutlookAddIn.Domain.Model
{
    public class DomainObjectFieldInfo : DomainObjectFieldInfoBase
    {
        public string[] Flags { get; set; }

        public string EnumType { get; set; }

        public DomainEnumInfo[] PossibleValues { get; set; }
    }
}