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
            }
            catch
            {
            }
            
            //return;

            string listagemPedidosCriados = "Lista de pedidos criados: " + "\r\n";            

            foreach (JObject document in Documents)
            {               
                {
                    string NewOrder = ObjectsHandler.CreateOrder(document);
                    Console.WriteLine("Pedido: " + NewOrder);
                    listagemPedidosCriados += "Pedido: " + NewOrder + "\r\n";                   
                }                                        
            }
           
            Console.WriteLine(listagemPedidosCriados);

            // grava a listagem de todos os pedidos criados 
            ObjectsHandler.Cosmos(listagemPedidosCriados);
            ObjectsHandler.CreateTask(listagemPedidosCriados);

        }
    }
}
