using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ShopifySharp;

namespace ShopifyCmd.ShopifyCommands
{
    public static class DefaultCommands
    {
        // Methods used as console commands must be public and must return a string
        public static string List(string what)
        {
            var serviceType = GetServiceType(what);

            if (serviceType == null)
                return string.Format(ConsoleFormatting.Indent(2) +
                "say what? I don't understand {0} ", what);

            object serviceInstance = Activator.CreateInstance(serviceType, new object[] { AppSettings.MyShopifyUrl, AppSettings.AccessToken });
            MethodInfo methodInfo = serviceType.GetMethod("ListAsync");

            var parametersArray = new object[] {null};
            var result = methodInfo.Invoke(serviceInstance, parametersArray);


            if ((result.GetType()).GetProperty("Result") != null)
            {
                var resultValue = (result.GetType()).GetProperty("Result").GetValue(result);

                 return FormatResponseFromList(((IEnumerable<ShopifyObject>)resultValue).ToList(),
                            new List<string>() { "Id", "Title", "Topic","Key","FirstName","LastName","OrderId","Event","Src","Address","Value","Code","Amount","CreatedAt" });
                        


            }
            return string.Format(ConsoleFormatting.Indent(2) +
                "say what? I don't understand {0} ", what);

        }




        // Methods used as console commands must be public and must return a string
        public static string Delete(string what, long id)
        {
            var serviceType = GetServiceType(what);

            if (serviceType == null)
                return string.Format(ConsoleFormatting.Indent(2) +
                "say what? I don't understand {0} ", what);

            object serviceInstance = Activator.CreateInstance(serviceType, new object[] { AppSettings.MyShopifyUrl, AppSettings.AccessToken });
            MethodInfo methodInfo = serviceType.GetMethod("DeleteAsync");

            var parametersArray = new object[] { id };
            var result = methodInfo.Invoke(serviceInstance, parametersArray);

            if ((result.GetType()).GetProperty("Result") != null)
            {
                var resultValue = (result.GetType()).GetProperty("Result").GetValue(result);
                return string.Format("{0} with id {1} delete request sent", what, id);

            }

            return string.Format(ConsoleFormatting.Indent(2) +
                "Say what? I don't understand {0} ", what);

        }

        public static string Get(string what, long id)
        {
            var serviceType = GetServiceType(what);

            if (serviceType == null)
                return string.Format(ConsoleFormatting.Indent(2) +
                "say what? I don't understand {0} ", what);

            object serviceInstance = Activator.CreateInstance(serviceType, new object[] { AppSettings.MyShopifyUrl, AppSettings.AccessToken });
            MethodInfo methodInfo = serviceType.GetMethod("GetAsync");

            var parametersArray = new object[] { id,null };
            var result = methodInfo.Invoke(serviceInstance, parametersArray);

            if ((result.GetType()).GetProperty("Result") != null)
            {
                var resultValue = (result.GetType()).GetProperty("Result").GetValue(result);

                var returnString = "";

                foreach (var propInfo in resultValue.GetType().GetProperties())
                {
                    var val = propInfo.GetValue(resultValue);
                    if (propInfo.PropertyType == typeof (System.DateTime))
                        val = ((DateTime) val).ToString("u");

                    returnString = returnString + propInfo.Name + ": " + (val?.ToString() ?? "null")+Environment.NewLine;
                }
                return returnString;

            }

            return string.Format(ConsoleFormatting.Indent(2) +
                "Say what? I don't understand {0} ", what);

        }



        private static Type GetServiceType(string what)
        {
            if (what.EndsWith("s"))
                what = what.Remove(what.Length - 1);

            return Assembly.Load("ShopifySharp").GetTypes().FirstOrDefault(n => n.Name == "Shopify" + what + "Service");
        }


        private static string FormatResponseFromList<T>(List<T> stuff, List<string> fieldNamesList  )
        {
            var responseString = "";
            var headerline = "";
            var lines = new List<string[]>();
            var headerList = new List<string>();



            foreach (var item in stuff)
            {

                foreach (var fieldName in fieldNamesList)
                {
                    var pinfo = item.GetType().GetProperties().FirstOrDefault(p => p.Name== fieldName);
                    if (pinfo != null)
                    {
                        if (! headerList.Contains(fieldName))
                            headerList.Add(fieldName);

                        responseString = string.Concat(responseString,
                            pinfo.GetValue(item).ToString(),
                            "\t"
                            );
                    }

                }
                responseString = responseString + Environment.NewLine;

            }

            foreach (var fieldName in headerList)
            {
                headerline = string.Concat(headerline, fieldName, "\t");
            }

            return headerline + Environment.NewLine + responseString;

            /*
                var getMethod = pinfo.GetGetMethod();
                    if (getMethod.ReturnType.IsArray)
                    {

                        var arrayObject = getMethod.Invoke(item, null);
                        foreach (object element in (Array) arrayObject)
                        {
                            foreach (PropertyInfo arrayObjPinfo in element.GetType().GetProperties())
                            {
                                Console.WriteLine(arrayObjPinfo.Name + ":" +
                                                arrayObjPinfo.GetGetMethod().Invoke(element, null).ToString());
                            }
                        }
                    }*/

            
        }
    }
}
