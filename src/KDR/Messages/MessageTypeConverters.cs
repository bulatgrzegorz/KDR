using System;
using System.Collections.Concurrent;
using System.Text;

namespace KDR.Messages
{
    public static class MessageTypeConverters
    {
        private static readonly ConcurrentDictionary<Type, string> _typeNames;
        private static readonly ConcurrentDictionary<string, Type> _nameTypes;

        static MessageTypeConverters()
        {
            _typeNames = new ConcurrentDictionary<Type, string>();
            _nameTypes = new ConcurrentDictionary<string, Type>();
        }

        public static string GetTypeName(Type bodyType)
        {
            //TODO:
            if (!_typeNames.TryGetValue(bodyType, out var result))
            {
            }

            return result;
        }

        public static Type GetNameType(string name)
        {
            //TODO:
            if (!_nameTypes.TryGetValue(name, out var result))
            {
            }

            return result;
        }

        ////https://github.com/rebus-org/Rebus/blob/13da53596ccd89b6ca9fbcdf066f5de6a93df9d0/Rebus/Internals/Shims.cs
        private static string GetSimpleAssemblyQualifiedName(Type type)
        {
            //TODO: Test on .net core
            return BuildSimpleAssemblyQualifiedName(type, new StringBuilder()).ToString();
        }

        private static StringBuilder BuildSimpleAssemblyQualifiedName(Type type, StringBuilder sb)
        {
            if (!type.IsGenericType)
            {
                sb.Append($"{type.FullName}, {type.Assembly.GetName().Name}");
                return sb;
            }

            if (!type.IsConstructedGenericType)
            {
                return sb;
            }

            var fullName = type.FullName ?? "???";
            var requiredPosition = fullName.IndexOf("[", StringComparison.Ordinal);
            var name = fullName.Substring(0, requiredPosition);
            sb.Append($"{name}[");

            var arguments = type.GetGenericArguments();
            for (var i = 0; i < arguments.Length; i++)
            {
                sb.Append(i == 0 ? "[" : ", [");
                BuildSimpleAssemblyQualifiedName(arguments[i], sb);
                sb.Append("]");
            }

            sb.Append($"], {type.Assembly.GetName().Name}");

            return sb;
        }
    }
}
