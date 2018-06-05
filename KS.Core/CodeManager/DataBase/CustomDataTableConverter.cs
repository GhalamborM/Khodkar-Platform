﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KS.Core.CodeManager.DataBase
{
    class CustomDataTableConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DataTable);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DataTable table = (DataTable)value;
            JArray array = new JArray();
            foreach (DataRow row in table.Rows)
            {
                JObject obj = new JObject();
                foreach (DataColumn col in table.Columns)
                {
                    //if (col.ColumnName == "Id") continue;  // skip Id column

                    if (row[col] != null && row[col] != DBNull.Value)
                    {
                        //string[] values = row[col].ToString().Split(',');
                        string[] values = new []{ row[col].ToString()} ;
                        obj.Add(col.ColumnName, JArray.FromObject(values));
                    }
                    else
                    {
                        obj.Add(col.ColumnName, JValue.CreateNull());
                    }
                }
                array.Add(obj);
            }
            array.WriteTo(writer);
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
