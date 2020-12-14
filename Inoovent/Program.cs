using Newtonsoft.Json.Linq;
using System;

namespace Inoovent
{
    class Program
    {
        static void Main(string[] args)
        {

            DateTime data = DateTime.Today.AddDays(-4);
            int recorrencia = 0;

            switch (data.Day.ToString())
            {
                case "1":
                    recorrencia = 11147708;
                    break;

                case "10":
                    recorrencia = 11147707;
                    break;

                case "20":
                    recorrencia = 11147706;
                    break;

                default:
                    Console.WriteLine("Não encontrou a data");
                    break;
            }            

            JArray Documents = ObjectsHandler.Document(recorrencia);

            foreach (JObject document in Documents)
            {
                string NewOrder = ObjectsHandler.CreateOrder(document);
                Console.WriteLine(NewOrder);
            }

        }
    }
}
