#region Importa Bibliotecas
using Interpretador.XML.Siemens;
using PLC.Program;
using System.Xml.Serialization;
using Xml.Registro;
using Tags;
using SIM;
#endregion

GetProgram GetProgram = new GetProgram();
BlockProgram block = new BlockProgram();
BlockProgram block2 = new BlockProgram();
GeraRoteiro roteiro = new GeraRoteiro();
LerValores lerValores = new LerValores();
List<Seq> Seqs = new List<Seq>();

block = GetProgram.Main(@"D:\༼ つ ◕_◕ ༽つ\Automacao_de_teste\Automação de teste\Automação de teste\XML\Exemplo0.xml");
//new Serializer(@"D:\༼ つ ◕_◕ ༽つ\Automacao_de_teste\Automação de teste\Automação de teste\XML\Xml_teste.xml",block);

//block2 = Desserializer.Main(@"D:\༼ つ ◕_◕ ༽つ\Automacao_de_teste\Automação de teste\Automação de teste\XML\Xml_teste.xml");

roteiro.Main(block.internal_tags_list);

Console.WriteLine("Preencha a sequencia, quando estiver terminado precione enter");
Console.ReadKey();

Seqs = lerValores.Main();

    foreach(var aux_seq in Seqs)
    {
        foreach (var aux in aux_seq.Seqlist)
        {
            var Input  = block.internal_tags_list[0].Find(X => X.tag_name == aux.TagName);
            if (Input != null)
            {
                if (aux.Value.ToString() == "1" | aux.Value.ToString() == "true")
                {
                    Input.value = true;
                }
                else if (aux.Value.ToString() == "0" | aux.Value.ToString() == "false") 
                {
                    Input.value = false;
                }
            }
            else
            {
                if (block.internal_tags_list[2].Count > 0)
                {
                    Input = block.internal_tags_list[2].Find(X => X.tag_name == aux.TagName);
                    if (Input != null)
                    {
                        if (aux.Value.ToString() == "1" | aux.Value.ToString() == "true")
                        {
                            Input.value = true;
                        }
                        else if (aux.Value.ToString() == "0" | aux.Value.ToString() == "false")
                        {
                            Input.value = false;
                        }
                    }
                }
                }
            }
    bool resultado = block.networks_list[0].and.Result(true);

    foreach (Tag output in block.internal_tags_list[1])
    {
        aux_seq.Resultados.Add(new SeqValue(output.tag_name, output.value.ToString()));
        Console.WriteLine($"\nSaida {output.tag_name}");
        Console.WriteLine(output.value);
    }
    

   

    Console.WriteLine("\n====================================================");

}
gravaResult gravaResult = new gravaResult();
gravaResult.main(Seqs);


//foreach (Tag input in block.internal_tags_list[0])
//{
//    string aux;
//    bool aux2;
//    Console.WriteLine($"Digite valor da Tag : {input.tag_name}");

//    aux = Console.ReadLine();
//    if (aux == "true")
//    {
//        aux2 = true;
//    }
//    else
//    {
//        aux2 = false;
//    }

//    input.value = aux2;
//}

//if (block.internal_tags_list[2].Count > 0)
//{
//    foreach (Tag input in block.internal_tags_list[2])
//    {
//        string aux;
//        bool aux2;

//        Console.WriteLine($"Digite valor da Tag : {input.tag_name}");

//        aux = Console.ReadLine();
//        if (aux == "true")
//        {
//            aux2 = true;
//        }
//        else
//        {
//            aux2 = false;
//        }

//        input.value = aux2;
//    }
//}
Console.WriteLine("FIM");
Console.ReadKey();
