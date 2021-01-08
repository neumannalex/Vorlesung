using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Vorlesung.Client.Pages.Stocks.Components
{
    public partial class CompanyOverview : ComponentBase
    {
        [Parameter]
        public AlphaVantageCompanyOverview Data { get; set; }

        public List<CompanyProperty> Properties { get; set; } = new List<CompanyProperty>();

        protected override void OnParametersSet()
        {
            if(Data != null)
            {
                Properties = new List<CompanyProperty>();

                var props = Data.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                foreach(var prop in props.OrderBy(x => x.Name))
                {
                    if (prop.PropertyType != typeof(string))
                        continue;

                    if (prop.Name == "Description")
                        continue;

                    if (!prop.CanRead || !prop.CanWrite)
                        continue;

                    var mget = prop.GetGetMethod(false);
                    var mset = prop.GetSetMethod(false);

                    if (mget == null || mset == null)
                        continue;

                    Properties.Add(
                        new CompanyProperty
                        {
                            Property = prop.Name,
                            Value = (string)prop.GetValue(Data)
                        });
                }
            }
            
        }
    }

    public class CompanyProperty
    {
        public string Property { get; set; }
        public string Value { get; set; }
    }
}
