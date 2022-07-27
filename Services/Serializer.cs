using System;
using System.Collections.Generic;
using System.Reflection;

namespace DevTrust_Task.Services
{
    public class Service : IServices
    {
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

            Type myType = obj.GetType();

            IList<PropertyInfo> props =
                new List<PropertyInfo>(myType.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                string propertyName = prop.Name.ToLower();
                if (data.ContainsKey(propertyName))
                {
                    if (prop.PropertyType == typeof (string))
                        prop.SetValue(obj, data[propertyName]);
                    else if (prop.PropertyType == typeof (long))
                        prop.SetValue(obj, long.Parse(data[propertyName]));
                    else if (prop.PropertyType == typeof(Int64))

                        prop.SetValue(obj, 1);
                    else
                    {
                        prop.SetValue(obj,
                            Deserialize(prop, data[propertyName]));
                    }
                        
                }
                else if (propertyName is "id") prop.SetValue(obj, 1);
                else prop.SetValue(obj, null);
            }
            return obj;
        }
    }
}
