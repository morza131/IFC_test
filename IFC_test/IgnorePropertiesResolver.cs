using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IFC_test
{
    internal class IgnorePropertiesResolver : DefaultContractResolver
    {
        private readonly HashSet<string> _propertiesToIgnore;

        public IgnorePropertiesResolver(params string[] propertiesToIgnore)
        {
            _propertiesToIgnore = new HashSet<string>(propertiesToIgnore, StringComparer.OrdinalIgnoreCase);
        }
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (_propertiesToIgnore.Contains(property.PropertyName))
            {
                property.ShouldSerialize = instance => false;
            }

            return property;
        }
    }
}
