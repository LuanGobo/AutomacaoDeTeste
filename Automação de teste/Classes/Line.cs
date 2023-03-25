using Operant;
using XML.wire;
using System.Xml.Serialization;
using System;
using System.Collections;

namespace Line
{
    public class Network
    {
        //[XmlElement("AND")]
        public And and = new And();
        public Network () { }
    }

    public class And
    {
        //[XmlElement("OR")]
        public List<Or> or_list = new List<Or>();
        public List<Contact> Contact_List = new List<Contact>();
        public bool result;
        public bool Result(bool value_in)
        {
            bool aux_result = value_in;
            foreach (Contact sum_contact in this.Contact_List)
            {
                if (sum_contact.contact_Type == "Contact")
                {
                    aux_result = aux_result & (sum_contact.tag.value ^ sum_contact.negated);
                }
            }

            foreach (Or sum_or_contact in this.or_list)
            {
                aux_result = aux_result & sum_or_contact.Result(aux_result);

            }

            foreach (Contact sum_contact in this.Contact_List)
            {
                if (sum_contact.contact_Type == "Coil")
                {
                    sum_contact.tag.value = aux_result;
                }
            }

            this.result = aux_result;
            return aux_result;
        }
        public And() { }

        public And(Contact contact) 
        {
            this.Contact_List.Add(contact);
        }
    }

    public class Or
    {
        public List<And> and_list = new List<And>();
        public bool result;

        public bool Result(bool value_in)
        {
            bool aux_result = false;
            foreach (And sum_contact in this.and_list)
            {
                
                aux_result = aux_result | sum_contact.Result(value_in); ;
            }

            this.result = aux_result;
            return aux_result;
        }

        public Or() { }

    }
}