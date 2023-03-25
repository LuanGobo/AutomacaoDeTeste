#region Importa Bibliotecas
using System.Xml;
using System.Xml.XPath;
using PLC;
using Operant;
#endregion

namespace Interpretador.NavegaXML
{
    public class NavigatorXML
    {

        public XPathNavigator Start_Navigator(string caminho_documento)
        {
            XPathNavigator navigator;

            #region Abre Arquivo
            XPathDocument document = new XPathDocument(caminho_documento);
            navigator = document.CreateNavigator();
            #endregion

            return navigator;
        }

        public XmlReader GetSubTree(XPathNavigator navigator, string localName, string namespaceURI = null)
        {

            #region Navega ate Tags
            navigator.MoveToFollowing(localName, namespaceURI);
            XmlReader XML_SubTree = navigator.ReadSubtree();
            #endregion

            return XML_SubTree;
        }
    }
}