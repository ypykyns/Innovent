using Newtonsoft.Json.Linq;
using System;

namespace Inoovent
{
    class Program
    {
        static void Main(string[] args)
        {

            // criar forma de pegar a data de recorrência
            // 01 - 11147708
            //10 - 11147707
            //20 - 11147706

            int idDataRecorrencia = 11147707;


            JArray Quotes = ObjectsHandler.GetQuotes(11147707);

            foreach (JObject quote in Quotes)
            {
                string NewOrder = ObjectsHandler.CreateOrder(quote);
                Console.WriteLine(NewOrder);
            }

        }
    }
}
