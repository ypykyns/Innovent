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
                    // todas as datas
                    // Document = RequestHandler.MakePloomesRequest($"Documents?$filter=(((TemplateId+eq+187646)+and+(Deal/StatusId+eq+2)))&$expand=OtherProperties($select=FieldKey,DecimalValue,BoolValue),Sections($select=Code,Total;$expand=OtherProperties($select=FieldKey,DecimalValue),Products($select=ProductId,Quantity,UnitPrice,Total,Discount;$expand=OtherProperties($select=FieldKey,DecimalValue,DateTimeValue)))&$select=Id,ContactId,DealId,OwnerId,CreateDate", Method.GET);

                    //selecionando a data de recorrência    
                    Document = RequestHandler.MakePloomesRequest($"Documents?$filter=(((TemplateId+eq+187646)+and+(Deal/StatusId+eq+2)+and+((Deal/OtherProperties/any(o:+o/FieldId+eq+161188+and+(o/IntegerValue+eq+{IdDataRecorrência}))))))&$expand=OtherProperties($select=FieldKey,DecimalValue,BoolValue),Sections($select=Code,Total;$expand=OtherProperties($select=FieldKey,DecimalValue),Products($select=ProductId,Quantity,UnitPrice,Total,Discount;$expand=OtherProperties($select=FieldKey,DecimalValue,DateTimeValue)))&$select=Id,ContactId,DealId,OwnerId,CreateDate", Method.GET);
                }

                catch
                {
                }

                return Document;
            }
            catch (Exception ex)
            {
                string error = "Source ===> " + ex.Source + "\r\n" + "Erro na execução da aplicação ===> " + ex.Message + "\r\n" +
                   "Método origem da exception ===> " + ex.TargetSite + "\r\n" + "Stack Trace ===> " + ex.StackTrace;

                Console.WriteLine(error);
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
                Order.Add("OwnerId", (int)Document["OwnerId"]);

                JArray OtherProperties = Document["OtherProperties"] as JArray;

                decimal dolarPtax = 0;
                bool fixarDolar = true;

                foreach (JObject property in OtherProperties)
                {
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


                if (!fixarDolar)
                {
                    bool hasProp = false;

                    foreach (JObject property in OtherProperties)
                    {
                        // Dolar PTAX
                        if (property["FieldKey"].ToString() == "document_949F6816-27C0-409D-96BB-6496B0730C78")
                        {
                            property["FieldKey"] = "order_DF26989A-F4B9-46A2-90A8-FDAE70BE4F9C";
                            property["DecimalValue"] = dolarPtax;
                            hasProp = true;
                            continue;
                        }
                    }

                    if (!hasProp)
                    {
                        JObject dptax = new JObject();
                        dptax.Add("FieldKey", "order_DF26989A-F4B9-46A2-90A8-FDAE70BE4F9C");
                        dptax.Add("DecimalValue", dolarPtax);

                        OtherProperties.Add(dptax);
                    }
                }
                else
                {
                    foreach (JObject property in OtherProperties)
                    {
                        // Dolar PTAX
                        if (property["FieldKey"].ToString() == "document_949F6816-27C0-409D-96BB-6496B0730C78")
                        {
                            property["FieldKey"] = "order_DF26989A-F4B9-46A2-90A8-FDAE70BE4F9C";
                            continue;
                        }
                    }
                }


                JArray Sections = Document["Sections"] as JArray;

                decimal totalBlocoZero = 0;
                decimal totalBlocoUm = 0;
                decimal custoTotal = 0;
                decimal valorTotalProduto = 0;

                foreach (JObject section in Sections)
                {

                    if (section["Code"].ToString() == "1")
                    {

                        try
                        {
                            foreach (JObject product in section["Products"])
                            {

                                foreach (JObject item in product["OtherProperties"])
                                {
                                    // custo unitário
                                    if (item["FieldKey"].ToString() == "document_product_F0825502-FC8A-430E-A153-8E502E06A8E6")
                                    {
                                        item["FieldKey"] = "order_table_product_7AE72CF2-C261-4F60-A5BD-FBC053F731D8";
                                        continue;
                                    }

                                    // custo total
                                    if (item["FieldKey"].ToString() == "document_product_AC89A6B5-E447-48B9-B444-15F70CBD7B6D")
                                    {
                                        item["FieldKey"] = "order_table_product_DD511660-35FD-488D-A0F9-CFBDF93284DD";

                                        if (!fixarDolar)
                                        {
                                            custoTotal += (decimal)item["DecimalValue"];
                                        }
                                        continue;
                                    }

                                    // valor unitário
                                    if (item["FieldKey"].ToString() == "document_product_57F27E95-4061-4B61-8F01-AF395CBB2EEC")
                                    {
                                        product["UnitPrice"] = item["DecimalValue"];

                                        if (!fixarDolar)
                                        {
                                            // unit price esta vindo null
                                            valorTotalProduto += ((decimal)product["Quantity"] * (decimal)product["UnitPrice"]);
                                        }

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
                            // total do bloco convertido
                            if (otherProp["FieldKey"].ToString() == "document_section_96112BD3-9CAD-4821-A75F-38916EA90C30")
                            {
                                if (!fixarDolar)
                                {
                                    otherProp["FieldKey"] = "order_table_D8AE5FC0-E643-440C-BE5C-3BE1439BCE7A";
                                    otherProp["DecimalValue"] = valorTotalProduto * dolarPtax;
                                }
                                else
                                {
                                    otherProp["FieldKey"] = "order_table_D8AE5FC0-E643-440C-BE5C-3BE1439BCE7A";

                                }

                                continue;
                            }

                            if (otherProp["FieldKey"].ToString() == "document_section_E262E7E9-C4B2-4F6D-9D9A-B927936F0D6B")
                            {
                                otherProp["FieldKey"] = "order_table_BA0D4E64-3E9E-4796-A1EC-069FAF6CFD13";
                                continue;
                            }

                            // total custo bloco
                            if (otherProp["FieldKey"].ToString() == "document_section_BB27BBB5-C3B9-4DA0-A463-14B0EC5C828D")
                            {
                                if (!fixarDolar)
                                {
                                    otherProp["FieldKey"] = "order_table_34EB08A2-E001-400E-AD7F-B32DB7D5DC72";
                                    otherProp["DecimalValue"] = custoTotal * dolarPtax;
                                }
                                else
                                {
                                    otherProp["FieldKey"] = "order_table_34EB08A2-E001-400E-AD7F-B32DB7D5DC72";

                                }
                                continue;
                            }
                        }
                        if (!fixarDolar)
                        {
                            totalBlocoUm = valorTotalProduto * dolarPtax;
                            section["Total"] = totalBlocoUm;
                        }
                    }
                }

                if (!fixarDolar)
                {
                    Order["Amount"] = (decimal)(totalBlocoUm + totalBlocoZero);
                }


                Order.Add("OtherProperties", OtherProperties);
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
            catch (Exception ex)
            {
                string error = "Source ===> " + ex.Source + "\r\n" + "Erro na execução da aplicação ===> " + ex.Message + "\r\n" +
                   "Método origem da exception ===> " + ex.TargetSite + "\r\n" + "Stack Trace ===> " + ex.StackTrace;

                Console.WriteLine(error);

                return "Erro na criação do Pedido => " + Document.ToString();
            }

        }

        public static decimal GetDolarPtaxVenda()
        {
            try
            {
                JArray cotacoes = new JArray();
                int count = 0;

                while (cotacoes.Count < 1 && count < 10)
                {
                    // primeiro pesquisa pela cotação do dia = se não encontrar vai subtraindo
                    //os dias até encontrar o primeiro dia com uma cotação válida


                    DateTime date = DateTime.Now.AddDays(-count);
                    string datePTax = date.ToString("MM-dd-yyyy");

                    cotacoes = RequestHandler.DolarPtax(datePTax);
                    count++;
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




    }
}
