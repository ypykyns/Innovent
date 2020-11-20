using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inoovent
{
    class ObjectsHandler
    {
        public static JArray GetQuotes(int IdDataRecorrência)
        {
            try
            {
                JArray Quotes = new JArray();

                try
                {
                    Quotes = RequestHandler.MakePloomesRequest($"Quotes?$filter=((((Deal/OtherProperties/any(o:+o/FieldId+eq+161188+and+(o/IntegerValue+eq+{IdDataRecorrência}))))+and+(Deal/StatusId+eq+2)))&$expand=Sections($select=Code,Total;$expand=OtherProperties($filter=FieldKey+eq+'quote_section_D26683BF-1F77-4611-A0FC-321D1E5C4228' or FieldKey+eq+'quote_section_B0E1D33D-B950-4964-BA21-14928D8F4176';$select=FieldKey,DecimalValue),Products($select=ProductId,Quantity,UnitPrice,Total,Discount;$expand=OtherProperties($filter=FieldKey+eq+'quote_product_30B614C7-9793-4AF6-B772-17BE1CE0823B' or FieldKey+eq+'quote_product_42C167A1-A6D6-41FE-900A-9A78C5EF7781';$select=FieldKey,DecimalValue)))&$select=Id,ContactId,DealId", Method.GET);
                }
                catch
                {
                }

                return Quotes;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error on GetQuotes Method ==> " + ex.Message);
                throw ex;
            }

        }

        public static string CreateOrder(JObject Quote)
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

    }
}
