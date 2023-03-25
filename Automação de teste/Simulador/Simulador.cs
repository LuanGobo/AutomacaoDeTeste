#region Importa Bibliotecas
using PLC.Program;
using Tags;
using ClosedXML.Excel;
#endregion


namespace SIM
{
    public class SeqValue
    {
        public string TagName;
        public string Value;

        public SeqValue(string TagName,string Value)
        {
            this.TagName = TagName;
            this.Value = Value;
        }

    }
    public class Seq
    {
        public List<SeqValue> Seqlist = new List<SeqValue>();
        public List<SeqValue> Resultados = new List<SeqValue>();
    }

    public class GeraRoteiro
    {
        static void GenerateHeader(IXLWorksheet worksheet)
        {
            worksheet.Cell("A1").Value = "Tag";
            worksheet.Cell("B1").Value = "Datatype";
        }

        public void Main(List<List<Tag>> tags)
        {
            String filePathName = System.IO.Directory.GetCurrentDirectory() + "\\Roteiro_de_teste.xlsx";
            if (File.Exists(filePathName))
            {
                File.Delete(filePathName);
            }
            using (var workbook = new XLWorkbook())
            {

                var planilha = workbook.Worksheets.Add("Inputs");

                int line = 1;
                GenerateHeader(planilha);
                line++;

                foreach (var tag in tags[0])
                {
                    planilha.Cells("A" + line).Value = tag.tag_name;
                    planilha.Cells("B" + line).Value = tag.Datatype;
                    line++;
                }
                foreach (var tag in tags[2])
                {
                    planilha.Cells("A" + line).Value = tag.tag_name;
                    planilha.Cells("B" + line).Value = tag.Datatype;
                    line++;
                }

                workbook.SaveAs(filePathName);
            }
        }
    }

    public class LerValores
    {


        public List<Seq> Main()
        {
            int quantida_de_sequencia = 0;
            String filePathName = System.IO.Directory.GetCurrentDirectory() + "\\Roteiro_de_teste.xlsx";
            List<Seq> Seq_List = new List<Seq>();
            
            if (File.Exists(filePathName))
            {
                var workbook = new XLWorkbook(filePathName);
                var nonEmptyDataRows = workbook.Worksheet(1).RowsUsed();

                foreach (var dataRow in nonEmptyDataRows)
                {
                    if (dataRow.RowNumber() == 1)
                    {
                        int i = 2;
                        bool Fim=false;

                        dataRow.CellCount();
                        while (Fim == false)
                        {
                            if (dataRow.Cell(i).Value.ToString().Contains("Seq"))
                            {
                                Seq auxseq = new Seq();

                                foreach (var dataRow2 in nonEmptyDataRows)
                                {
                                    if (dataRow2.RowNumber() > 1)
                                    {
                                        auxseq.Seqlist.Add(new SeqValue(dataRow2.Cell(1).Value.ToString(), dataRow2.Cell(i).Value.ToString()));
                                    }
                                    
                                }
                                Seq_List.Add(auxseq);
                            }
                            else if (dataRow.Cell(i).Value.ToString() == "")
                            {
                                Fim = true;
                            }
                            i++;
                        }
                       
                    }

                }
            }
            return Seq_List;
        }

    }
    public class gravaResult 
    {
        static void GenerateHeader(IXLWorksheet worksheet)
        {
            worksheet.Cell("A1").Value = "Tag";
        }


        public void main(List<Seq> Seq) 
        {
            String filePathName = System.IO.Directory.GetCurrentDirectory() + "\\Roteiro_de_teste.xlsx";
            if (File.Exists(filePathName))
            {
                var workbook = new XLWorkbook(filePathName);

                using  (workbook)
                {
                    IXLWorksheet planilha ;
                    if (!(workbook.Worksheets.Contains("Result")))
                    {
                       planilha = workbook.Worksheets.Add("Results");
                    }

                       planilha = workbook.Worksheet("Results");

                    int line = 1;
                    GenerateHeader(planilha);
                    bool aux_first = true;
                    int aux_seq_count = 1;
                    foreach (var i in Seq)
                    {
                        
                        planilha.Cells(planilha.LastColumnUsed().ColumnRight().FirstCell().ToString()).Value = "Seq " + aux_seq_count + " Result";
                        line = 2;
                        aux_seq_count++;
                        foreach (var j in i.Resultados)
                        {
                            if (aux_first = true)
                            {
                                planilha.Cells("A" + line).Value = j.TagName;
                            }
                            planilha.Cells(planilha.LastColumnUsed().ColumnLetter().ToString() + line).Value = j.Value;
                            line++;
                        }
                        aux_first = false;
                    }

                    workbook.SaveAs(filePathName);
                }
            }
        }

        }

}
