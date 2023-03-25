#region Importa Bibliotecas
using PLC.Program;
using System.Xml.Serialization;
#endregion

namespace Xml.Registro
{
    public class Serializer
    {
        public void Main(string arquivoXml, BlockProgram block)
        {
            using (var stream = new StreamWriter(arquivoXml))
            {
                XmlSerializer serializador = new XmlSerializer(typeof(BlockProgram));
                serializador.Serialize(stream, block);
            }
        }
        public Serializer(string arquivoXml, BlockProgram block) 
        { 
            Main(arquivoXml, block);
        }

    }
    public class Desserializer
    {
        public static BlockProgram Main(string arquivoXml)
        {
            using (var stream = new StreamReader(arquivoXml))
            {
                XmlSerializer serializador = new XmlSerializer(typeof(BlockProgram));
                BlockProgram blockProgram = (BlockProgram)serializador.Deserialize(stream);
                return blockProgram;
            }
        }

    }
}
