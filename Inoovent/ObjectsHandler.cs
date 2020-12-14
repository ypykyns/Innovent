using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inoovent
{
    class ObjectsHandler
    {
        public static JArray Document(int IdDataRecorrência)
        {
            try
            {
                JArray Document = new JArray();

                try
                {
                    Document = RequestHandler.MakePloomesRequest($"Documents?$filter=(((TemplateId+eq+187646)+and+(Deal/StatusId+eq+2)+and+((Deal/OtherProperties/any(o:+o/FieldId+eq+161188+and+(o/IntegerValue+eq+{IdDataRecorrência}))))))&$expand=OtherProperties($select=FieldKey,DecimalValue,BoolValue),Sections($select=Code,Total;$expand=OtherProperties($select=FieldKey,DecimalValue),Products($select=ProductId,Quantity,UnitPrice,Total,Discount;$expand=OtherProperties($select=FieldKey,DecimalValue,DateTimeValue)))&$select=Id,ContactId,DealId", Method.GET);
                }
                catch
                {
                }

                return Document;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error on GetOrders Method ==> " + ex.Message);
                throw ex;
            }
        }        

        public static string CreateOrder(JObject Document)
        {
            string orderId = "";

            try
            {
                JObject Order = new JObject();

                Order.Add("ContactId", (int)Document["ContactId"]);
                Order.Add("DealId", (int)Document["DealId"]);                

                JArray OtherProperties = Document["OtherProperties"] as JArray;

                decimal dolarPtax = 0;
                bool fixarDolar = true;

                foreach (JObject property in OtherProperties)
                {                    
                    // Dolar PTAX
                    if (property["FieldKey"].ToString() == "document_949F6816-27C0-409D-96BB-6496B0730C78")
                    {
                        property["FieldKey"] = "order_DF26989A-F4B9-46A2-90A8-FDAE70BE4F9C";
                        continue;
                    }

                    // Fixar Dolar 
                    if (property["FieldKey"].ToString() == "document_D7E10E53-4855-474A-974A-F052ECC0C5B2")
                    {
                        fixarDolar = (bool)property["BoolValue"];

                        if (!fixarDolar)
                        {
                            // busca a cotação do dia
                            dolarPtax = GetDolarPtaxVenda();
                        }
                    }

                    // Valor Total do Documento
                    if (property["FieldKey"].ToString() == "document_6A10EFCB-1375-4B27-85D6-8DB260CF9FBE")
                    {
                        Order.Add("Amount", (decimal)property["DecimalValue"]);
                        continue;

                    }

                    
                }              


                JArray Sections = Document["Sections"] as JArray;

                foreach (JObject section in Sections)
                {
                    if (section["Code"].ToString() == "0")
                    {
                        try
                        {
                            foreach (JObject product in section["Products"])
                            {
                                foreach (JObject item in product["OtherProperties"])
                                {
                                    if (item["FieldKey"].ToString() == "document_product_A1A169AD-2E6C-4A78-ABC9-2FCDDFA62BBC")
                                    {
                                        item["FieldKey"] = "order_table_product_7AE72CF2-C261-4F60-A5BD-FBC053F731D8";
                                        continue;
                                    }

                                    if (item["FieldKey"].ToString() == "document_product_AC89A6B5-E447-48B9-B444-15F70CBD7B6D")
                                    {
                                        item["FieldKey"] = "order_table_product_DD511660-35FD-488D-A0F9-CFBDF93284DD";
                                        continue;
                                    }

                                    if (item["FieldKey"].ToString() == "document_product_84AEE89F-091B-4395-954A-0982AFCEFA0D")
                                    {
                                        item["FieldKey"] = "order_table_product_F6A7B96B-0474-4547-99BD-6F24DF07199F";
                                        continue;
                                    }

                                }
                            }
                        }
                        catch
                        {
                        }

                        //OtherProperties
                        foreach (JObject otherProp in section["OtherProperties"])
                        {

                            if (otherProp["FieldKey"].ToString() == "document_section_BE26BCEA-7411-48C9-BB31-C2E8FD8B07EC")
                            {
                                otherProp["FieldKey"] = "order_table_24FAD56F-8BE1-4518-B0EA-FF0314E5E58C";
                                continue;
                            }

                            if (otherProp["FieldKey"].ToString() == "document_section_E262E7E9-C4B2-4F6D-9D9A-B927936F0D6B")
                            {
                                otherProp["FieldKey"] = "order_table_BA0D4E64-3E9E-4796-A1EC-069FAF6CFD13";
                                continue;
                            }

                        }

                    }

                    if (section["Code"].ToString() == "1")
                    {
                        try
                        {
                            foreach (JObject product in section["Products"])
                            {
                                foreach (JObject item in product["OtherProperties"])
                                {
                                    if (item["FieldKey"].ToString() == "document_product_F0825502-FC8A-430E-A153-8E502E06A8E6")
                                    {
                                        item["FieldKey"] = "order_table_product_7AE72CF2-C261-4F60-A5BD-FBC053F731D8";
                                        continue;
                                    }

                                    if (item["FieldKey"].ToString() == "document_product_AC89A6B5-E447-48B9-B444-15F70CBD7B6D")
                                    {
                                        item["FieldKey"] = "order_table_product_DD511660-35FD-488D-A0F9-CFBDF93284DD";
                                        continue;
                                    }
                                }

                            }
                        }
                        catch
                        {
                        }

                        //OtherProperties
                        foreach (JObject otherProp in section["OtherProperties"])
                        {

                            if (otherProp["FieldKey"].ToString() == "document_section_96112BD3-9CAD-4821-A75F-38916EA90C30")
                            {
                                otherProp["FieldKey"] = "order_table_D8AE5FC0-E643-440C-BE5C-3BE1439BCE7A";
                                continue;
                            }

                            if (otherProp["FieldKey"].ToString() == "document_section_E262E7E9-C4B2-4F6D-9D9A-B927936F0D6B")
                            {
                                otherProp["FieldKey"] = "order_table_BA0D4E64-3E9E-4796-A1EC-069FAF6CFD13";
                                continue;
                            }

                            if (otherProp["FieldKey"].ToString() == "document_section_BB27BBB5-C3B9-4DA0-A463-14B0EC5C828D")
                            {
                                otherProp["FieldKey"] = "order_table_34EB08A2-E001-400E-AD7F-B32DB7D5DC72";
                                continue;
                            }
                        }

                    }
                }




                Order.Add("Sections", Sections);

                JArray NewOrder = RequestHandler.MakePloomesRequest($"Orders", Method.POST, Order);

                try
                {
                    orderId = NewOrder[0]["Id"].ToString();
                }
                catch
                {
                }




                return orderId;
            }
            catch
            {
                return "Erro na criação do Pedido => " + Document.ToString();
            }

        }

        public static decimal GetDolarPtaxVenda()
        {
            try
            {
                JArray cotacoes = new JArray();

                while (cotacoes.Count < 1)
                {
                    // primeiro pesquisa pela cotação do dia = se não encontrar vai subtraindo
                    //os dias até encontrar o primeiro dia com uma cotação válida

                    int x = 0;
                    DateTime date = DateTime.Now.AddDays(-x);
                    string datePTax = date.ToString("MM-dd-yyyy");

                    cotacoes = RequestHandler.DolarPtax(datePTax);
                    x++;
                }

                decimal cotacaoPtax = 0;

                foreach (JObject cotacao in cotacoes)
                {
                    cotacaoPtax = (decimal)cotacao["cotacaoVenda"];
                }

                return cotacaoPtax;

            }
            catch
            {
                return 0;
            }
        }



        /*
        public static string CreateOrder_(JObject Quote)
        {
            string orderId = "";

            try
            {
                JObject Order = new JObject();

                Order.Add("ContactId", (int)Quote["ContactId"]);
                Order.Add("DealId", (int)Quote["DealId"]);

                JArray Sections = Quote["Sections"] as JArray;

                foreach (JObject section in Sections)
                {
                    if (section["Code"].ToString() == "0")
                    {
                        try
                        {
                            foreach (JObject product in section["Products"])
                            {
                                foreach (JObject item in product["OtherProperties"])
                                {
                                    if (item["FieldKey"].ToString() == "quote_product_42C167A1-A6D6-41FE-900A-9A78C5EF7781")
                                    {
                                        item["FieldKey"] = "order_table_product_7AE72CF2-C261-4F60-A5BD-FBC053F731D8";
                                        continue;
                                    }

                                    if (item["FieldKey"].ToString() == "quote_product_30B614C7-9793-4AF6-B772-17BE1CE0823B")
                                    {
                                        item["FieldKey"] = "order_table_product_DD511660-35FD-488D-A0F9-CFBDF93284DD";
                                        continue;
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                    }

                    if (section["Code"].ToString() == "1")
                    {
                        try
                        {
                            foreach (JObject product in section["Products"])
                            {
                                foreach (JObject item in product["OtherProperties"])
                                {
                                    if (item["FieldKey"].ToString() == "quote_product_42C167A1-A6D6-41FE-900A-9A78C5EF7781")
                                    {
                                        item["FieldKey"] = "order_table_D8AE5FC0-E643-440C-BE5C-3BE1439BCE7A";
                                        continue;
                                    }

                                    if (item["FieldKey"].ToString() == "quote_product_30B614C7-9793-4AF6-B772-17BE1CE0823B")
                                    {
                                        item["FieldKey"] = "order_table_34EB08A2-E001-400E-AD7F-B32DB7D5DC72";
                                        continue;
                                    }
                                }

                            }
                        }
                        catch
                        {
                        }

                    }
                }


                Order.Add("Sections", Sections);

                JArray NewOrder = RequestHandler.MakePloomesRequest($"Orders", Method.POST, Order);

                try
                {
                    orderId = NewOrder[0]["Id"].ToString();
                }
                catch
                {
                }




                return orderId;
            }
            catch
            {
                return "Erro na criação do Pedido => " + Quote.ToString();
            }

        }
        */

    }
}
