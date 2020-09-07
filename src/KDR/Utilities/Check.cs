using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly : InternalsVisibleTo("KDR.Transport.ServiceBus")]

namespace KDR.Utilities
{
    ////https://github.com/dotnet/efcore/blob/master/src/Shared/Check.cs
    internal static class Check
    {
        public static T NotNull<T>(T value, string parameterName)
        {
#pragma warning disable IDE0041 // Use 'is null' check
            if (!ReferenceEquals(value, objB : null))
#pragma warning restore IDE0041 // Use 'is null' check
            {
                return value;
            }

            NotEmpty(parameterName, nameof(parameterName));

            throw new ArgumentNullException(parameterName);
        }

        public static IReadOnlyList<T> NotEmpty<T>(IReadOnlyList<T> value, string parameterName)
        {
            NotNull(value, parameterName);

            if (value.Count != 0)
            {
                return value;
            }

            NotEmpty(parameterName, nameof(parameterName));

            throw new ArgumentException("CollectionArgumentIsEmpty", parameterName);
        }

        public static string NotEmpty(string value, string parameterName)
        {
            Exception e = null;
            if (value is null)
            {
                e = new ArgumentNullException(parameterName);
            }
            else if (value.Trim().Length == 0)
            {
                e = new ArgumentException("ArgumentIsEmpty", parameterName);
            }

            if (e == null)
            {
                return value;
            }

            NotEmpty(parameterName, nameof(parameterName));

            throw e;
        }

        public static string NullButNotEmpty(string value, string parameterName)
        {
            if (value is null || value.Length != 0)
            {
                return value;
            }

            NotEmpty(parameterName, nameof(parameterName));

            throw new ArgumentException("ArgumentIsEmpty", parameterName);
        }

        public static IReadOnlyList<T> HasNoNulls<T>(IReadOnlyList<T> value, string parameterName)
        where T : class
        {
            NotNull(value, parameterName);

            if (value.All(e => e != null))
            {
                return value;
            }

            NotEmpty(parameterName, nameof(parameterName));

            throw new ArgumentException(parameterName);
        }
    }
}