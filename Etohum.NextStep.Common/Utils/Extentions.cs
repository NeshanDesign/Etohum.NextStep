using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Etohum.NextStep.Common.Utils
{
    public static class Extentions
    {
        public static string FullTrace(this Exception e)
        {
            return e.InnerException == null 
                ? string.Format(@"[E]:{2} Exception: {0}{3} StackTrace: {1}{3} ", e.Message, e.StackTrace, DateTime.Now, Environment.NewLine)
                : string.Format(@"[E]:{3} Exception: {0}{4} StackTrace: {1}{4} InnerException:{2}", e.Message, e.StackTrace, e.InnerException.FullTrace(), DateTime.Now, Environment.NewLine);
        }
        public static byte[] GetBytes(this string str)
        {
            return System.Text.Encoding.UTF8.GetBytes(str);
        }

        public static string GetString(this IEnumerable<byte> binaries)
        {
            return System.Text.Encoding.UTF8.GetString(binaries.ToArray());
        }
    }


    public static class AutomapperExtentions
    {
            public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
            {
                var sourceType = typeof(TSource);
                var destinationType = typeof(TDestination);
                var existingMaps = Mapper.Configuration.GetAllTypeMaps().First(x => x.SourceType == sourceType && x.DestinationType == destinationType);
                foreach (var property in existingMaps.GetUnmappedPropertyNames())
                {
                    expression.ForMember(property, opt => opt.Ignore());
                }
                return expression;
            }

            //
            /// <summary>
            /// Mapping source object to destination type, all none existing properties in destination will be ignored
            /// </summary>
            /// <typeparam name="TS">type of source object</typeparam>
            /// <typeparam name="TT">type of target object</typeparam>
            /// <param name="obj">source object</param>
            /// <returns>an object of target type with source object data</returns>
            public static TT Get<TS, TT>(this TS obj)
            {
                return Mapper.Map<TS, TT>(obj);
            }

            /// <summary>
            /// Mapping enumerable of source objects to enumerable of destination type, all none existing properties in destination will be ignored
            /// </summary>
            /// <typeparam name="TS">type of source object</typeparam>
            /// <typeparam name="TT">type of target object</typeparam>
            /// <param name="listOfSourceObjects">enumerable of source objects</param>
            /// <returns>enumurable of target objects</returns>
            public static IEnumerable<TT> Get<TS, TT>(this IEnumerable<TS> listOfSourceObjects)
            {
                var result = new List<TT>();
                foreach (var item in listOfSourceObjects)
                {
                    try
                    {
                        var x = item.Get<TS, TT>();
                        result.Add(x);
                    }
                    catch (Exception ex)
                    {
                        LogManager.Error(ex);
                    }
                }
                return result;
            }
    }
}
