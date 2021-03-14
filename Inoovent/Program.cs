using Newtonsoft.Json.Linq;
using System;

namespace Inoovent
{
    class Program
    {
        static void Main(string[] args)
        {
            JArray Documents = ObjectsHandler.Document();

            try
            {
                Console.WriteLine(Documents.Count + " documentos encontrados.");
                return;
            }
            catch
            {
            }

            foreach (JObject document in Documents)
            {
                if ((int)document["Id"] == 80718231)
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
}
