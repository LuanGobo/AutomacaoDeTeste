using Line;
using Tags;
using System.Xml.Serialization;

namespace PLC.Program
{
    public class BlockProgram
    {
        //[XmlElement("TagsInternas")]
        public List<List<Tag>> internal_tags_list { get; set; }

        //[XmlElement("Networks")]
        public List<Network> networks_list { get; set; }

        public BlockProgram() { }


    }
}
