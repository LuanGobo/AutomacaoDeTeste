#region Importa Bibliotecas
using System.Xml;
using System.Xml.XPath;
using Interpretador.NavegaXML;
using Tags;
using Line;
using Operant;
using XML.wire;
using XML.Parts;
using PLC.Program;
#endregion

namespace Interpretador.XML.Siemens
{
    public class GetProgram
    {
        public BlockProgram Main(string File_Address)
        {
            GetTags gettags = new GetTags();
            BlockProgram program = new BlockProgram();
            GetLine getLine = new GetLine();

            program.internal_tags_list = gettags.ListTags(File_Address);
            program.networks_list = getLine.GetNetworks(program.internal_tags_list, File_Address);

            return program;
         }

        public GetProgram() { }


    }

    public class GetTags : NavigatorXML
    {
        #region Define Variaveis
        public List<List<Tag>> List_Tags;
        #endregion

        //Coleta Lista de todas as TAGs
        public List<List<Tag>> ListTags(string File_Address)
        {
            List<List<Tag>> temp_tag_list = new List<List<Tag>>();
            List<Tag> tags_Input_list = new List<Tag>();
            List<Tag> tags_Output_list = new List<Tag>();
            List<Tag> tags_InOut_list = new List<Tag>();
            List<Tag> tags_Temp_list = new List<Tag>();
            List<Tag> tags_Constant_list = new List<Tag>();
            List<Tag> tags_Return_list = new List<Tag>();

            #region Navega no XML
            XPathNavigator Navigator = Start_Navigator(File_Address);
            XmlReader XML_SubTree_Tags = GetSubTree(Navigator, "Sections", "http://www.siemens.com/automation/Openness/SW/Interface/v3");
            #endregion

            #region Adiciona Tags
            while (XML_SubTree_Tags.Read())
            {

                if (XML_SubTree_Tags.Name.ToString() == "Section" & !XML_SubTree_Tags.IsEmptyElement)
                {
                    switch (XML_SubTree_Tags.GetAttribute("Name").ToString())
                    {
                        case "Input":
                            tags_Input_list = GetTagList(XML_SubTree_Tags.ReadSubtree(), XML_SubTree_Tags.GetAttribute("Name").ToString(), File_Address);
                            temp_tag_list.Add(tags_Input_list);
                            break;
                        case "Output":
                            tags_Output_list = GetTagList(XML_SubTree_Tags.ReadSubtree(), XML_SubTree_Tags.GetAttribute("Name").ToString(), File_Address);
                            temp_tag_list.Add(tags_Output_list);
                            break;
                        case "InOut":
                            tags_InOut_list = GetTagList(XML_SubTree_Tags.ReadSubtree(), XML_SubTree_Tags.GetAttribute("Name").ToString(), File_Address);
                            temp_tag_list.Add(tags_InOut_list);
                            break;
                        case "Temp":
                            tags_Temp_list = GetTagList(XML_SubTree_Tags.ReadSubtree(), XML_SubTree_Tags.GetAttribute("Name").ToString(), File_Address);
                            temp_tag_list.Add(tags_Temp_list);
                            break;
                        case "Constant":
                            tags_Constant_list = GetTagList(XML_SubTree_Tags.ReadSubtree(), XML_SubTree_Tags.GetAttribute("Name").ToString(), File_Address);
                            temp_tag_list.Add(tags_Constant_list);
                            break;
                        case "Return":
                            tags_Return_list = GetTagList(XML_SubTree_Tags.ReadSubtree(), XML_SubTree_Tags.GetAttribute("Name").ToString(), File_Address);
                            temp_tag_list.Add(tags_Return_list);
                            break;
                    }
                }
            }
            #endregion

            #region retorna Lista
            return temp_tag_list;
            #endregion
        }

        // Coleta as Tags 
        public List<Tag> GetTagList(XmlReader XML_SubTree,string Mode,string File_Address)
        {
            List<Tag> temp_tag_list = new List<Tag>();
            List<Access> access_list = GetAccess(File_Address);

            if (XML_SubTree.HasAttributes & XML_SubTree.IsStartElement())
            {
                while (XML_SubTree.Read()) 
                { 
                    if (XML_SubTree.Name.ToString() == "Member")
                    {
                        Access aux_access = access_list.Find(x => x.tag_name == XML_SubTree.GetAttribute("Name"));
                        if (aux_access != null)
                        {
                            Tag aux_tag = temp_tag_list.Find(x => x.tag_name == XML_SubTree.GetAttribute("Name"));
                            if (aux_tag == null)
                            {
                                temp_tag_list.Add(new(XML_SubTree.GetAttribute("Name"), XML_SubTree.GetAttribute("Datatype"), XML_SubTree.GetAttribute("Accessibility"), aux_access.UId, aux_access.Scope, Mode));
                            }             
                        }
                    }
                }
            }
            XML_SubTree.Close();


            return temp_tag_list;
        }

        public List<Part> GetPart(string File_Address)
        {
            List<Part> part_list = new List<Part>();

            #region Navega no XML
            XPathNavigator Navigator = Start_Navigator(File_Address);
            XmlReader XML_SubTree_Parts = GetSubTree(Navigator, "FlgNet", "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v3");
            #endregion

            while (XML_SubTree_Parts.Read())
            {
                if (XML_SubTree_Parts.Name.ToString() == "Part" & XML_SubTree_Parts.IsStartElement())
                {

                    string Contact_Type = XML_SubTree_Parts.GetAttribute("Name");
                    string Contact_UId = XML_SubTree_Parts.GetAttribute("UId");
                    bool negated = false;
                    XmlReader XML_Aux = XML_SubTree_Parts.ReadSubtree();
                    while (XML_Aux.Read())
                    {
                        if (XML_Aux.Name.ToString() == "Negated" & XML_Aux.IsStartElement())
                        {

                            negated = true;
                        }
                    }
                    part_list.Add(new Part(Contact_Type, Contact_UId,negated));
                }
            }
            return part_list;
        }

        public List<Access> GetAccess(string File_Address)
        {
            List<Access> access_list = new List<Access>();

            #region Navega no XML
            XPathNavigator Navigator = Start_Navigator(File_Address);
            XmlReader XML_SubTree_Parts = GetSubTree(Navigator, "FlgNet", "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v3");
            #endregion

            while (XML_SubTree_Parts.Read())
            {
                if (XML_SubTree_Parts.Name.ToString() == "Access" & XML_SubTree_Parts.IsStartElement())
                {
                    string Scope = XML_SubTree_Parts.GetAttribute("Scope");
                    string Tag_Uid = XML_SubTree_Parts.GetAttribute("UId");
                    XmlReader XML_Aux = XML_SubTree_Parts.ReadSubtree();
                    while (XML_Aux.Read())
                    {
                        if (XML_SubTree_Parts.Name.ToString() == "Component" & XML_SubTree_Parts.IsStartElement())
                        {
                            Access aux_access = access_list.Find(x => x.tag_name == XML_SubTree_Parts.GetAttribute("Name"));
                            if (aux_access != null)
                            {
                                aux_access.UId += "," + Tag_Uid;
                            } 
                            else
                            {
                                string tag_name = XML_SubTree_Parts.GetAttribute("Name");
                                access_list.Add(new Access(Scope, Tag_Uid, tag_name));
                            }

                        }
                    }
                }
            }
            return access_list;
        }
    }

    public class GetLine : NavigatorXML
    {
        #region Define Variaveis
        int or_count = 0;
        int and_count = 0;
        int network_count = 0;
        #endregion

        public List<Network> GetNetworks(List<List<Tag>> tags,string File_Address)
        {
            List<Network> networks = new List<Network>();
            List<Contact> contact = new List<Contact>();
            List<Wire> wires = new List<Wire>();
            GetTags getTags = new GetTags();
            bool aux_or = false;
            int cont_or = 0;

            wires = GetWires(File_Address);
            contact = GetContact(wires,tags, File_Address);
            networks.Add(new Network());


            foreach (Wire wire in wires)
            {

                if(wire.namecon_list.Count >= 1)
                {


                    List<NameCon> aux_wire = wire.namecon_list.FindAll(x => x.Name == "in");

                    if (aux_wire.Count() > 1)
                    {
                        networks[0].and.or_list.Add(new Or());
                        foreach (NameCon nameCon in wire.namecon_list)
                        {
                            if (nameCon.Name == "in")
                            {
                                Contact aux_contact = contact.Find(x => x.contact_UId == nameCon.UId);
                                if (aux_contact != null)
                                {
                                    networks[0].and.or_list[cont_or].and_list.Add(new And(aux_contact));
                                    aux_or = true;
                                }
                            }
                        }

                    }
                    else
                    {
                        string aux_Uid_anterior = "";
                        
                        foreach (NameCon nameCon in wire.namecon_list)
                        {

                            if (nameCon.Name == "out")
                            {
                                aux_Uid_anterior = nameCon.UId;
                            }

                            if (nameCon.Name == "in")
                            {
                                Contact aux_contact;
                                if (networks[0].and.or_list.Count >0)
                                {   

                                    aux_contact = contact.Find(x => x.contact_UId == nameCon.UId);
                                    if (aux_contact != null)
                                    {
                                        And aux_or_and = networks[0].and.or_list[cont_or].and_list.Find(x => x.Contact_List.Exists(x => x.contact_UId == aux_Uid_anterior));
                                        if (aux_or_and != null)
                                        {
                                            aux_or_and.Contact_List.Add(aux_contact);
                                        }
                                        else
                                        {
                                            networks[0].and.Contact_List.Add(aux_contact);
                                        }
                                    }
                                }
                                else
                                {
                                    aux_contact = contact.Find(x => x.contact_UId == nameCon.UId);
                                    if (aux_contact != null)
                                    {
                                        networks[0].and.Contact_List.Add(aux_contact);
                                    }

                                }
                            }
                        }
                    }              
                }
            }


            return networks;
        }


        public List<Wire> GetWires(string XMLFileAddress)
        {
            List<Wire> wires = new List<Wire>();

            #region Navega no XML
            XPathNavigator Navigator = Start_Navigator(XMLFileAddress);
            XmlReader XML_SubTree_Wire = GetSubTree(Navigator, "Wires", "http://www.siemens.com/automation/Openness/SW/NetworkSource/FlgNet/v3");
            #endregion

            #region Extrai Lista de Wires
            while (XML_SubTree_Wire.Read())

                //UId do Contato
                if (XML_SubTree_Wire.Name.ToString() == "Wire" & XML_SubTree_Wire.IsStartElement())
                {
                    wires.Add(new Wire());

                    string wire_UId = XML_SubTree_Wire.GetAttribute("UId");
                    XmlReader XML_Aux = XML_SubTree_Wire.ReadSubtree();

                    while (XML_Aux.Read())
                    {

                        //Verifica se é o primeiro contato e suas conexões
                        if (XML_SubTree_Wire.Name.ToString() == "Powerrail" & XML_SubTree_Wire.IsStartElement())
                        {
                            wires[wires.Count - 1].powerrail = true;
                        }

                        //controi conexões do contato
                        if (XML_SubTree_Wire.Name.ToString() == "NameCon" & XML_SubTree_Wire.IsStartElement())
                        {
                            wires[wires.Count - 1].namecon_list.Add(new NameCon(XML_SubTree_Wire.GetAttribute("UId"), XML_SubTree_Wire.GetAttribute("Name")));
                        }

                        //Coleta UId de Tag do contato
                        if (XML_SubTree_Wire.Name.ToString() == "IdentCon" & XML_SubTree_Wire.IsStartElement())
                        {
                            wires[wires.Count - 1].identcon_list.Add(new IdentCon(XML_SubTree_Wire.GetAttribute("UId")));
                        }
                    }
                }
            #endregion


            return wires;
        }

        public List<Contact> GetContact(List<Wire> wires_list, List<List<Tag>> list_tags,string File_Address)
        {
            List<Contact> contact_list = new List<Contact>();
            GetTags getTags = new GetTags();
            List<Part> part_list = getTags.GetPart(File_Address);

            foreach (Wire wire in wires_list)
            {
                if (wire.namecon_list.Count > 0 & wire.identcon_list.Count == 0)
                {
                    foreach (NameCon nameCon in wire.namecon_list)
                    {
                        if (nameCon.Name == "in")
                        {
                            
                            Part aux_part = part_list.Find(x => x.Contact_UId == nameCon.UId);
                            contact_list.Add(new Contact() {powerrail = wire.powerrail, contact_UId = nameCon.UId, negated = aux_part.Negated , contact_Type = aux_part.Contact_Type });
                        }
                        if (nameCon.Name == "out")
                        {   
                            if (wire.namecon_list.Exists(x => x.Name == "in"))
                            {
                                NameCon aux_namecon = wire.namecon_list.Find(x => x.Name == "in");
                                Contact aux_contact = contact_list.Find(x => x.contact_UId == nameCon.UId);
                                if (aux_contact != null)
                                {
                                    aux_contact.wire_out_UId = aux_namecon.UId;
                                }      
                            }
                            else if (wire.namecon_list.Exists(x => x.Name.Contains("in")))
                            {
                                NameCon aux_namecon = wire.namecon_list.Find(x => x.Name.Contains("in"));
                                Wire aux_wire = wires_list.Find(x => x.namecon_list.Contains(new NameCon(aux_namecon.UId, "out"), new ComparerDados()));
                                if (aux_wire.namecon_list.Exists(x => x.UId == aux_namecon.UId & x.Name == "out"))
                                {
                                    NameCon aux_namecon1 = aux_wire.namecon_list.Find(x => x.Name == "in");
                                    Contact aux_contact = contact_list.Find(x => x.contact_UId == nameCon.UId);
                                    aux_contact.wire_out_UId = aux_namecon1.UId;

                                }
                            }
                        }
                    }
                }

                if (wire.identcon_list.Count > 0)
                {
                    foreach (List<Tag> tags in list_tags)
                    {   
                        Tag aux_tag = tags.Find(x => x.tag_UID.Contains(wire.identcon_list[0].UId));
                        if (aux_tag != null)
                        {
                            Contact contact = contact_list.Find(x => x.contact_UId == wire.namecon_list[0].UId);
                            contact.tag = aux_tag;
       
                        }
                    }
                }
            }
            return contact_list;
        }

        //constructor
        public GetLine() { }
        }
    }
