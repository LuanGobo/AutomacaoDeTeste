using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML.wire
{
    public class Wire
    {
        public bool powerrail = false;
        public List<NameCon> namecon_list = new List<NameCon>();
        public List<IdentCon> identcon_list = new List<IdentCon>();
        public Wire() { }

    }
    public class NameCon
    {
        public string UId { get; set; }
        public string Name { get; set; }


        public NameCon() { }

        public NameCon(string UId,String Name)
        {
            this.UId = UId;
            this.Name = Name;
        }    

    }
    public class IdentCon
    {
        public string UId { get; set; }

        public IdentCon() { }

        public IdentCon(string UId)
        {
            this.UId= UId;
        }
    }
    public class ComparerDados : IEqualityComparer<NameCon>
    {
        public bool Equals(NameCon x, NameCon y)
        {
            return (x.Name.Equals(y.Name)) &&
                (x.UId.Equals(y.UId));
        }

        public int GetHashCode(NameCon obj)
        {

            return 0;
        }
        public ComparerDados() { }
       
    }
}

