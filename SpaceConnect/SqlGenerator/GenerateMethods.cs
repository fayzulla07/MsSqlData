using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SqlData.SqlGenerator
{
    public class GenerateMethods
    {
        public GenerateMethods()
        {

        }
        // Determine the primary key from the [Key] attribute from the model
        public string GetKeyName<T>()
        {
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in propertyInfos)
            {
                var attribute = Attribute.GetCustomAttribute(property, typeof(KeyAttribute)) as KeyAttribute;
                if (attribute != null)
                {
                    return property.Name;
                }

            }
            return null;
        }
        // Generate part insert
        public string GeneratePartInsert<T>(T entity, bool allowNulls = false)
        {
            List<string> fields = new List<string>();
            if (!allowNulls)
            {
                foreach (var item in GetEntity<T>())
                {
                    if (GetPropValue(entity, item) != null)
                    {
                        fields.Add(item);
                    }
                }
            }
            else
                fields = GetEntity<T>().ToList();

            fields.Remove(GetKeyName<T>());
            var result = string.Empty;
            var sb = new StringBuilder($"INSERT INTO {typeof(T).Name} (");
            var propertiesNamesDef = fields.Select(a => "[" + a + "]").ToArray();
            string camps = string.Join(",", propertiesNamesDef);
            sb.Append($"{camps}) VALUES (");
            string[] parametersCampsCol = fields.Select(a => $"@{a}").ToArray();
            string campsParameter = string.Join(",", parametersCampsCol);
            sb.Append($"{campsParameter})");
            result = sb.ToString();
            return result;
        }
        // Generate part select
        public string GenerateSelect<T>(string param = "*")
        {
            var result = string.Empty;
            var sb = new StringBuilder("SELECT ");
            string separator = $",{Environment.NewLine}";
            string selectPart = string.Empty;
            if (param == "*")
            {
                selectPart = string.Join(separator, GetEntity<T>().Select(a => "[" + a + "]"));
            }
            else
            {
                selectPart = string.Join(separator, SplitParam(param).Select(a => a));
            }
            sb.AppendLine(selectPart);
            string fromPart = $"FROM [{typeof(T).Name}]";
            sb.Append(fromPart);
            result = sb.ToString();
            return result;

        }
        // Generate part update
        public string GenerateUpdate<T>(T entity, bool allowNulls = false)
        {
            List<string> fields = new List<string>();
            if (!allowNulls)
            {
                foreach (var item in GetEntity<T>())
                {
                    if (GetPropValue(entity, item) != null)
                    {
                        fields.Add(item);
                    }
                }
            }
            else
                fields = GetEntity<T>().ToList();
            fields.Remove(GetKeyName<T>());
            var sb = new StringBuilder($"UPDATE {typeof(T).Name} SET ");

            var propertiesSet = fields.Select(a => $"[{a}] = @{a}").ToArray();

            var strSet = string.Join(",", propertiesSet);

            var where = GenerateById(GetKeyName<T>());

            sb.Append($" {strSet} {where} ");

            var result = sb.ToString();

            return result;
        }
        // Generate part delete
        public string GenerateDelete<T>(string ParamName)
        {
            var where = GenerateById(ParamName);

            var result = $"DELETE FROM {typeof(T).Name} {where} ";

            return result;
        }
        // get all entities from model 
        private string[] GetEntity<Entity>()
        {
            return typeof(Entity).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(a => !a.PropertyType.FullName.StartsWith("System.Collections") && a.PropertyType.FullName.StartsWith("System.")).Select(a => a.Name).ToArray();
        }
        private static string[] SplitParam(string param)
        {
            return param.Trim().Split(new char[] { ',' }).Select(a => "[" + a.Trim() + "]").ToArray();
        }
        // get property value from model by reflection
        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        public string Generate_where<T>(T entity, bool allowNulls = false)
        {
            var initialSelect = GenerateSelect<T>();
            var where = GenerateWhere<T>(entity, allowNulls);
            var result = $" {initialSelect} {where}";
            return result;
        }
        // Generate part ById
        private string GenerateById(string FilterName)
        {
            return " WHERE " + "[" + FilterName + "]" + " = @" + FilterName;
        }
        public string GenerateById<T>(string filter = null)
        {
            if(string.IsNullOrEmpty(filter))
            return " WHERE " + "[" + GetKeyName<T>() + "]" + " = @" + GetKeyName<T>();
            else
                return " WHERE " + "[" + filter + "]" + " = @" + filter;
        }

        // Generate part where
        private string GenerateWhere<T>(T entity, bool allowNulls = false)
        {
            List<string> fields = new List<string>();
            if (!allowNulls)
            {
                foreach (var item in GetEntity<T>())
                {
                    if (GetPropValue(entity, item) != null)
                    {
                        fields.Add(item);
                    }
                }
            }
            else
                fields = GetEntity<T>().ToList();
            fields.Remove(GetKeyName<T>());

            var filtersPksFields = fields.Select(a => a).ToArray();
            var propertiesWhere = filtersPksFields.Select(a => $"[{a}] = @{a}").ToArray();
            var strWhere = string.Join(" AND ", propertiesWhere);
            var result = $" WHERE {strWhere} ";
            return result;
        }
        // Generate Find
        public string GenerateFind<T>(T entity, string param = "*", bool allowNulls = false)
        {
            var initialSelect = GenerateSelect<T>(param);
            var where = GenerateWhere<T>(entity, allowNulls);
            var result = $" {initialSelect} {where}";
            return result;
        }

    }
}
