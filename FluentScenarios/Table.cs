using System;
using System.Collections.Generic;
using System.Dynamic;

namespace FluentScenarios
{
    public class Table
    {
        private int _columnCount;
        private Dictionary<int, string> _headers;
        
        public List<dynamic> Rows { get; private set; }

        public Table()
        {
            _columnCount = 0;
            _headers = new Dictionary<int, string>();
            Rows = new List<dynamic>();
        }
        
        public Table Headers(params string[] headers)
        {
            _columnCount = headers.Length;

            for (var i = 0; i < headers.Length; i++)
            {
                _headers[i] = headers[i];
            }
            
            return this;
        }

        public Table Row(params object[] values)
        {
            if (values.Length < _columnCount)
            {
                throw new RowItemCountMissMatch("Not enough items in the row!");
            }

            if (values.Length > _columnCount)
            {
                throw new RowItemCountMissMatch("Too many items in the row!");
            }

            dynamic row = new ExpandoObject();
            var rowAsDictionary = row as IDictionary<String, object>;
            for (var i = 0; i < _columnCount; i++)
            {
                rowAsDictionary[_headers[i]] = values[i];
            }
            Rows.Add(row);

            return this;
        }
        
    }

    public class RowItemCountMissMatch : Exception
    {
        public RowItemCountMissMatch(string message) : base(message: message){}
    }
}