using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Net.Http;
using System.Text;
using CsvHelper;
using HtmlAgilityPack;
namespace HtmlToCsv;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        HttpTextString("https://www.cik.bg/bg/epns2024/candidates/ep");
    }

    public static void HttpTextString(string Text){
     var  http = Text;
        var htmlWeb = new HtmlWeb();

            var csvRow = new List<CsvRow>();

          

        try{
          var httpText = htmlWeb.Load(Text);
          //System.Console.WriteLine(httpText.Text);
          string[] result = httpText.Text.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
          for (int i = 0; i < result.Length; i++)
          {
            if(result[i].Contains("""<li style="cursor: pointer"><strong>""")){
                string Temp = result[i];
                int start = 0;
                int end = 0;
                List<string> Lines = new List<string>();
                //Lines.Clear();
                
                bool trigger = false;
                for (int c = 0; c < Temp.Length; c++)
                {
                    if(Temp[c]=='g'){
                        start = c+2;
                        trigger = true;
                    }
                    if(Temp[c] =='<'&&trigger){
                        end = c;
                        break;
                    }
                }
                Temp = Temp.Substring(start,end-start);
                //Lines.Add(Temp);
                System.Console.WriteLine(Temp);
                var csv = new StringBuilder();
                for (int d = i+1; d < result.Length; d++)
                {
                    if(result[d].Contains("</ul>")){
                        i=d-1;
                      break;
                    }
                    else{
                        string temp2 = result[d];
                        bool trigger2 = false;
                        for (int c = 0; c < temp2.Length; c++)
                           { 
                             if(temp2[c]=='i')
                                {
                                start = c+2;
                                trigger2 = true;
                                }
                            if(temp2[c] =='<'&&temp2[c+1]=='/'&&trigger2)
                            {
                            end = c;
                            break;
                              }
                          }
                          temp2 = temp2.Substring(start,end-start);
                          Lines.Add(temp2);
                          
                          
                           //temp2 = "    " + temp2;
                          System.Console.WriteLine(temp2);
                        }
                       
                }
                
                 csvRow.Add(new CsvRow{Name = Temp,TextList = Lines});
               
            }
             
            
          }
          for (int ic = 0; ic < csvRow.Count; ic++)
                {
                    System.Console.WriteLine(csvRow[ic].ToString());
                }
                using (var writer = new StreamWriter($".\\test.csv"))
                foreach(var itemList in csvRow){                              
                {
                    string csv = itemList.Name+","+String.Join(",", itemList.TextList.Select(x => x.ToString()).ToArray()); 
                    writer.WriteLine(csv);          
                }
                }
        }
        catch(Exception ex){
            Console.WriteLine(ex.Message);

        }
    }

    public class CsvRow{
       public string Name {get;set;}= string.Empty;
       public List<string> TextList {get;set;}= new List<string>();

        public override string ToString()
        {
            StringBuilder stringTest = new StringBuilder();
            stringTest = stringTest.AppendLine(Name);
            for (int i = 0; i < TextList.Count; i++)
            {
                stringTest.AppendLine(TextList[i]+',');
            }
            return stringTest.ToString();
        }
    }
}
