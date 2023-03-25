using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML.Parts
{
    public class Access
    {
        public string Scope { get; set; }
        public string UId { get; set; }
        public string tag_name { get; set; }

    public Access() { }
    public Access(string Scope, string UId, string tag_name)
        {
            this.Scope = Scope;
            this.UId = UId;
            this.tag_name = tag_name;

        }

    }

    public class Part
    {
        public string Contact_Type { get; set; }
        public string Contact_UId { get; set; }
        public bool Negated { get; set; }



        public Part() { }

        public Part(string Contact_Type, string Contact_UId, bool Negated) 
        { 
            this.Contact_Type = Contact_Type;
            this.Contact_UId = Contact_UId;
            this.Negated = Negated;
        
        }



    }

}
