using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BeeGees_ReadNode.Facade.FastAccess
{
    public class CachedQueryString
    {
        public string QueryString { get; private set; }

        public List<CachedQueryParam> PlainQueryParameters { get; private set; }  // Regular ParameterName:ParameterValue pairs with no SHA-1 shit

        public Type Type { get; private set; }

        public CachedQueryString(Type type, string[] splits, List<CachedQueryParam> parameters)
        {
            QueryString = string.Join("/", splits);
            PlainQueryParameters = parameters;
            Type = type;
        }

        public CachedQueryString(string sender, Type messageType, params CachedQueryParam[] queryParams)
        {
            /*
               A Cached Query String (CQS) is a representation of a query from a specific source. It is an encoded version of the query that allows to determine
               if the request being sent has already been evaluated before.

               The common format for CQS is:

               START/{SENDER}/{REQUEST_TYPE}/SHA1({PARAM_NAME}={PARAM_VALUE})/.../.../END

                Paramenters' are SHA-1'd in order to avoid collisions with "/" because I am lazy to learn RegEx and exctract the query properties with them.

                TODO:
                    - DEFINITELY KEEP THE FORMAT HUMAN READABLE - aka use RegEx
                    - Given that parameter objects implement IComparable, in the query allow parameters to have formats like: {PARAMETER_NAME}<>{PARAMETER_VALUE}
                    - Wildcards
                    - Query reordering
                    - Parameter type deduction

                More complex comparsions can be implemented by adding new nodes:
                START/{SENDER}/{REQUEST_TYPE}/CustomerId=1/ReleaseDate<'01/02/2020'/ReleaseDate>'01/01/2020'/Status!=Open Beta/END

                NOTE: Parameter names MUST equal the names of each property in the stored object.
                      IE:
                      A class named Foo with Properties -> ID, Date, Name
                      MUST be represented with a query like
                      START/localhost/Foo/Date=2/2/2020/END
            */

            var sb = new StringBuilder();
            sb.Append("START/");
            sb.Append(sender + "/");
            sb.Append(messageType.Name + "/");
            using(var sha1 = SHA1.Create() )
            {
                foreach (var param in queryParams)
                {
                    var paramVal = sha1.ComputeHash(Encoding.ASCII.GetBytes($"{param.Name.ToLower()}={param.Value}"));   // yuck. Better to use Unicode but meeh
                    sb.Append($"{Convert.ToBase64String(paramVal).Replace("/", "_").Replace("+", "-")}/");  // yuck - Btw we use URL-safe encodings but keep the paddings because fuck it.
                }
            }
            sb.Append("END");

            QueryString = sb.ToString();
            PlainQueryParameters = queryParams.ToList();
            Type = messageType;
        }
    }
}
