namespace WebAPITest.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNet.OData.Query;
    using Newtonsoft.Json;
    using WebAPITest.Models;

    internal static class OdataQueryFilter
    {
        /// <summary>
        /// Extracts the oDataQueryOptions.Filter.RawValue.
        /// </summary>
        /// <param name="oDataQueryOptions">The odata query options.</param>
        /// <returns></returns>
        internal static List<OdataFilter> ExtractOdataFilter(ODataQueryOptions oDataQueryOptions)
        {
            string oDataFilterExpression = @"(?<Filter>" +
                                       "\n" + @"     (?<Resource>.+?)\s+" +
                                       "\n" + @"     (?<Operator>eq|ne|gt|ge|lt|le|add|sub|mul|div|mod)\s+" +
                                       "\n" + @"     '?(?<Value>.+?)'?" +
                                       "\n" + @")" +
                                       "\n" + @"(?:" +
                                       "\n" + @"    \s*$" +
                                       "\n" + @"   |\s+(?:or|and|not)\s+" +
                                       "\n" + @")" +
                                       "\n";
            Regex oDataFilterRegex = new Regex(oDataFilterExpression, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            string strTargetString = oDataQueryOptions.Filter.RawValue.TrimEnd(')').TrimStart('(');
            string strReplace = @"{ 'Resource' : '${Resource}'," + @"   'Operator' : '${Operator}'," + @"   'Value'    : '${Value}'},";
            var val = oDataFilterRegex.Replace(strTargetString, strReplace);
            var filterJson = "[" + val.TrimEnd(',') + "]"; //Make json array
            var oDataFilterObject = JsonConvert.DeserializeObject<List<OdataFilter>>(filterJson);
            return oDataFilterObject;
        }

    }
    internal class OdataFilter
    {
        public string Resource { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
    }

    public class companyProcessingController : ODataController
    {
        public IQueryable<Company> Get(ODataQueryOptions options)
        {
            if (options.Filter != null && !string.IsNullOrEmpty(options.Filter.RawValue))
            {
                var oDataFilterObject = OdataQueryFilter.ExtractOdataFilter(options);
                var name = oDataFilterObject.Find(x => x.Resource.ToUpper() == "NAME").Value;
            }

            var data = Company_rep.list;
            var queryable = options.ApplyTo(data);
            return queryable as IQueryable<Company>;
        }
    }
}
