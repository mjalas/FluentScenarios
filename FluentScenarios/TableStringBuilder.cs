using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using ConsoleTables;

namespace FluentScenarios
{
    public static class TableStringBuilder
    {
        public static string Build(Table table)
        {
            var firstRow = true;
            List<string> headers = new List<string>();
            List<List<object>> rows = new List<List<object>>();
            foreach (var row in table.Rows)
            {
                switch (row)
                {
                    case IDictionary<string, object> rowAsDictionary:
                    {
                        if (firstRow)
                        {
                            headers = rowAsDictionary.Select(entry => entry.Key).ToList();
                            firstRow = false;
                        }

                        var rowValues = rowAsDictionary.Select(entry => entry.Value).ToList();
                        rows.Add(rowValues);
                        break;
                    }
                }
            }
            var outputTable = new ConsoleTable(headers.ToArray());
            outputTable.Configure(o => o.EnableCount = false);
            foreach (var row in rows)
            {
                outputTable.AddRow(row.ToArray());
            }

            return outputTable.ToString();
        }
    }
}