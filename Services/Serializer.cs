using DevTrust_Task.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DevTrust_Task.Services
{
    public class Service : IServices
    {

        private readonly DevTrustContext _context;

        public Service(DevTrustContext context)
        {
            _context = context;
        }



        public string CutBothSideIfNeeded(string str)
        {
            if (str[0] != 34 && str[0] != 39 && str[0] != 123 && str[0] != 8216)
                return str; // if str doesnt start with ' " {

            return str.Substring(1, str.Length - 2);
        }

        public int CloseBracket(string str)
        {
            int b = 1;

            for (int i = 1; i < str.Length; i++)
            {
                if (str[i] == 123) b++;
                if (str[i] == 125) b--;
                if (b == 0) return i;
            }
            return 0;
        }

        public long GetId(Type search)
        {
            Type type = _context.GetType();

            IList<PropertyInfo> props =
                new List<PropertyInfo>(type.GetProperties());


            foreach (PropertyInfo prop in props)
            {
                if (prop.Name.Equals(search.Name))
                {
                    IQueryable<Models.Identification> dbSet = (IQueryable<Models.Identification>)prop.GetValue(_context);
                    try
                    {
                        return dbSet.Max(p => p.Id) + 1;
                    }
                    catch
                    {
                        return 1;
                    }
                }
            }
            return 1;


        }



        public Dictionary<string, dynamic> DeserializeToDictionary(string json)
        {
            json = CutBothSideIfNeeded(json);

            string
                key,
                value;
            string[] devided_json;
            Dictionary<string, dynamic> pairs =
                new Dictionary<string, dynamic>();

            while (json != null && json != "")
            {
                devided_json = json.Split(":", 2);
                Console.WriteLine("{0} {1}", devided_json[0], devided_json[1]);
                key = CutBothSideIfNeeded(devided_json[0].Trim()).ToLower();
                value = devided_json[1].Trim();

                if (value[0] == 123)
                {
                    int closeBracket = CloseBracket(value);
                    pairs[key] = value.Substring(0, closeBracket + 1);

                    if (closeBracket + 1 == value.Length)
                        json = null;
                    else
                        json =
                            value
                                .Substring(closeBracket + 1,
                                value.Length - closeBracket);
                }
                else
                {
                    try
                    {
                        pairs[key] =
                            CutBothSideIfNeeded(value.Split(",", 2)[0].Trim());
                        json = value.Split(",", 2)[1].Trim();
                    }
                    catch (System.Exception)
                    {
                        pairs[key] = CutBothSideIfNeeded(value.Trim());
                        json = null;
                    }
                }
            }

            return pairs;
        }

        public object Deserialize(dynamic obj, string json)
        {
            Dictionary<string, dynamic> data = DeserializeToDictionary(json);
            Type myType;
            try
            {
                myType = obj.PropertyType;
                obj = Activator.CreateInstance(myType);
            }
            catch (System.Exception)
            {
                myType = obj.GetType();
            }

            IList<PropertyInfo> props =
                new List<PropertyInfo>(myType.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                string propertyName = prop.Name.ToLower();
                if (data.ContainsKey(propertyName))
                {
                    if (prop.PropertyType == typeof(string))
                        prop.SetValue(obj, data[propertyName]);
                    else if (prop.PropertyType == typeof(long))
                        prop.SetValue(obj, long.Parse(data[propertyName]));
                    else
                    {
                        prop.SetValue(obj,
                            Deserialize(prop, data[propertyName]));

                    }

                }
                else if (propertyName is "id") prop.SetValue(obj, GetId(myType));
                else prop.SetValue(obj, null);
            }
            return obj;
        }


        public string Serialize(dynamic obj)
        {
            Type myType;
            string json = "{ ";
            try
            {
                myType = obj.PropertyType;
                obj = Activator.CreateInstance(myType);
            }
            catch (System.Exception)
            {
                myType = obj.GetType();
            }

            IList<PropertyInfo> props =
                new List<PropertyInfo>(myType.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                bool isString = prop.PropertyType == typeof(string);
                json += "\"" + prop.Name.ToLower() + "\":";
                if (prop.PropertyType.IsClass && !isString)
                            json += Serialize(prop.GetValue(obj)) + ",";

                else
                        {
                    if (isString) json += "\"";
                            json += prop.GetValue(obj).ToString();

                    if (isString) json += "\"";


                    json += ",";
                }

            }

            json = json.Substring(0, json.Length - 1) + "}";
            

            return json;
        }
    }
}
