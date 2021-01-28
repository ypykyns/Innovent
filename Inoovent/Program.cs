using Newtonsoft.Json.Linq;
using System;

namespace Inoovent
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime data = DateTime.Today;
            int recorrencia = 0;

            Console.WriteLine("Iniciou execução => Data de recorrência: " + data.Day.ToString());

            switch (data.Day.ToString())
            {
                case "1":
                    recorrencia = 11147708;
                    break;

                case "2":
                    recorrencia = 12207866;
                    break;

                case "3":
                    recorrencia = 12208762;
                    break;

                case "4":
                    recorrencia = 12208766;
                    break;

                case "5":
                    recorrencia = 12208768;
                    break;

                case "6":
                    recorrencia = 12208770;
                    break;

                case "7":
                    recorrencia = 12208773;
                    break;

                case "8":
                    recorrencia = 12208776;
                    break;

                case "9":
                    recorrencia = 12208779;
                    break;

                case "10":
                    recorrencia = 11147707;
                    break;

                case "11":
                    recorrencia = 12208782;
                    break;

                case "12":
                    recorrencia = 12208807;
                    break;

                case "13":
                    recorrencia = 12208817;
                    break;

                case "14":
                    recorrencia = 12208819;
                    break;

                case "15":
                    recorrencia = 12208822;
                    break;

                case "16":
                    recorrencia = 12208824;
                    break;

                case "17":
                    recorrencia = 12208827;
                    break;

                case "18":
                    recorrencia = 12208830;
                    break;

                case "19":
                    recorrencia = 12208832;
                    break;

                case "20":
                    recorrencia = 11147706;
                    break;

                case "21":
                    recorrencia = 12208837;
                    break;

                case "22":
                    recorrencia = 12208852;
                    break;

                case "23":
                    recorrencia = 12208855;
                    break;

                case "24":
                    recorrencia = 12208858;
                    break;

                case "25":
                    recorrencia = 12208859;
                    break;

                case "26":
                    recorrencia = 12208862;
                    break;

                case "27":
                    recorrencia = 12208864;
                    break;

                case "28":
                    recorrencia = 12208866;
                    break;

                case "29":
                    recorrencia = 12208869;
                    break;

                case "30":
                    recorrencia = 12208872;
                    break;

                case "31":
                    recorrencia = 12208875;
                    break;

                default:
                    Console.WriteLine("Não encontrou a data");
                    return;
            }

            Console.WriteLine("Id recorrência ==>" + recorrencia);

            JArray Documents = ObjectsHandler.Document(recorrencia);

            try
            {
                Console.WriteLine(Documents.Count + " documentos encontrados.");
            }
            catch
            {
            }


            foreach (JObject document in Documents)
            {
                DateTime dataCriacaoDoc = DateTime.Parse((document["CreateDate"].ToString()));
                DateTime dataAtual = DateTime.Today;

                if (dataCriacaoDoc.Month.ToString() != dataAtual.Month.ToString())
                {
                    string NewOrder = ObjectsHandler.CreateOrder(document);
                    Console.WriteLine("Pedido criado ==> " + NewOrder);
                }
                else
                {
                    if (dataCriacaoDoc.Year.ToString() != dataAtual.Year.ToString())
                    {
                        string NewOrder = ObjectsHandler.CreateOrder(document);
                        Console.WriteLine("Pedido criado ==> " + NewOrder);
                    }
                    else
                    {
                        Console.WriteLine("Pedido emitido no mês corrente, não emitir");
                    }
                }
            }

        }
    }
}
