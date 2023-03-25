
namespace Tags
{
    public class Tag
    {
        public string tag_UID { get; set; }
        public string tag_name { get; set; }
        public string Datatype { get; set; }
        public bool value { get; set; }
        public string Mode { get; set; }
        public string Scope { get; set; }
        public string Accessibility { get; set; }

        public Tag() { }
        public Tag(string tag_name,string DataType,string Accessibility,string tag_UID, string Scope,string Mode)
        {
            this.tag_name = tag_name;
            this.Datatype = DataType;
            this.Accessibility = Accessibility;
            this.tag_UID = tag_UID;
            this.Scope = Scope;
            this.Mode = Mode;
        }

    }
}
