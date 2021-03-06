﻿// Copyright 2012-2012 Chris Patterson
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file
// except in compliance with the License. You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
// ANY KIND, either express or implied. See the License for the specific language governing
// permissions and limitations under the License.
namespace FeatherVane.Visualization
{
    using System;
    using System.Text;
    using Internals.Caching;


    public class VaneTypeNameFormatter
    {
        readonly Cache<Type, string> _cache;
        readonly string _genericArgumentSeparator;
        readonly string _genericClose;
        readonly string _genericOpen;
        readonly string _namespaceSeparator;
        readonly string _nestedTypeSeparator;

        public VaneTypeNameFormatter()
            : this(",", "<", ">", ".", "+")
        {
        }

        public VaneTypeNameFormatter(string genericArgumentSeparator, string genericOpen, string genericClose,
            string namespaceSeparator, string nestedTypeSeparator)
        {
            _genericArgumentSeparator = genericArgumentSeparator;
            _genericOpen = genericOpen;
            _genericClose = genericClose;
            _namespaceSeparator = namespaceSeparator;
            _nestedTypeSeparator = nestedTypeSeparator;

            _cache = new ConcurrentCache<Type, string>(FormatTypeName);
        }

        public string GetTypeName(Type type)
        {
            return _cache[type];
        }

        string FormatTypeName(Type type)
        {
            if (type.IsGenericTypeDefinition)
                throw new ArgumentException("An open generic type cannot be used as a message name");

            var sb = new StringBuilder("");

            return FormatTypeName(sb, type, null);
        }

        string FormatTypeName(StringBuilder sb, Type type, string scope)
        {
            if (type.IsGenericParameter)
                return "";

            if (type.Namespace != null)
            {
                string ns = type.Namespace;
                if (!ns.Equals(scope) && !ns.StartsWith("FeatherVane") && !ns.StartsWith("System"))
                {
                    sb.Append(ns);
                    sb.Append(_namespaceSeparator);
                }
            }

//            if (type.IsNested)
//            {
//                FormatTypeName(sb, type.DeclaringType, type.Namespace);
//                sb.Append(_nestedTypeSeparator);
//            }

            if (type.IsGenericType)
            {
                string name = type.GetGenericTypeDefinition().Name;

                //remove `1
                int index = name.IndexOf('`');
                if (index > 0)
                    name = name.Remove(index);

                sb.Append(CleanName(name));
                sb.Append(_genericOpen);

                Type[] arguments = type.GetGenericArguments();
                for (int i = 0; i < arguments.Length; i++)
                {
                    if (i > 0)
                        sb.Append(_genericArgumentSeparator);

                    FormatTypeName(sb, arguments[i], type.Namespace);
                }

                sb.Append(_genericClose);
            }
            else
                sb.Append(CleanName(type.Name));

            return sb.ToString();
        }

        static string CleanName(string name)
        {
            var cleanName = name;
            const string sourcevane = "SourceVane";
            if (cleanName.EndsWith(sourcevane))
                cleanName = cleanName.Substring(0, cleanName.Length - sourcevane.Length);

            const string vane = "Vane";
            if (cleanName.EndsWith(vane))
                cleanName = cleanName.Substring(0, cleanName.Length - vane.Length);
            return cleanName;
        }
    }
}