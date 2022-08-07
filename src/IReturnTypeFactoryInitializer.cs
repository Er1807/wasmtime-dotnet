using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Wasmtime
{
    internal class Test<TReturn>
    {
        public static IReturnTypeFactory<TReturn> Create()
        {
            var types = GetTupleTypes().ToList();

            if (types.Count == 1)
            {
                return new NonTupleTypeFactory<TReturn>();
            }

            // All of the factories take parameters: <TupleType, Item1Type, Item2Type... etc>
            // Add TupleType to the start of the list
            types.Insert(0, typeof(TReturn));

            Type factoryType = GetFactoryType(types.Count - 1);
            return (IReturnTypeFactory<TReturn>)Activator.CreateInstance(factoryType.MakeGenericType(types.ToArray()))!;
        }

        protected static Type GetFactoryType(int arity)
        {
            return arity switch
            {
                2 => typeof(TupleFactory2<,,>),
                3 => typeof(TupleFactory3<,,,>),
                4 => typeof(TupleFactory4<,,,,>),
                5 => typeof(TupleFactory5<,,,,,>),
                6 => typeof(TupleFactory6<,,,,,,>),
                7 => typeof(TupleFactory7<,,,,,,,>),
                _ => throw new InvalidOperationException("Too many return types in tuple"),
            };
        }

        public static IReadOnlyList<Type> GetTupleTypes()
        {
            if (typeof(ITuple).IsAssignableFrom(typeof(TReturn)))
            {
                return typeof(TReturn).GetGenericArguments();
            }
            else
            {
                return new[] { typeof(TReturn) };
            }
        }

        public static MethodInfo GetCreateMethodInfo(int arity)
        {
            return typeof(ValueTuple)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(a => a.Name == "Create")
                .Where(a => a.ContainsGenericParameters && a.IsGenericMethod)
                .First(a => a.GetGenericArguments().Length == arity);
        }

        
    }
}
