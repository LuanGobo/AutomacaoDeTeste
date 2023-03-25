using System.Xml;
using System.Xml.Serialization;
using Tags;

namespace Operant
{
    public class Contact
    {
        public string contact_UId { get; set; }
        public string contact_Type { get; set; }
        public bool negated { get; set; }
        public bool powerrail { get; set; }
        public string wire_out_UId { get; set; }
        public Tag tag { get; set; }
        public Contact() {
        }
    }

    public class Result
    {
        public bool Input { get; set; }
        public bool Value { get; set; }
        public bool Output { get; set; }

    }
}

