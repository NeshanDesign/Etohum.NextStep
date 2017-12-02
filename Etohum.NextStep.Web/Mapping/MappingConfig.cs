using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Etohum.NextStep.Common.Dto;
using Etohum.NextStep.Common.Utils;

namespace Etohum.NextStep.Web.Mapping
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            AutoMapper.Mapper.Initialize(cfg => {
                cfg.CreateMap<ViewModel.SubscribeViewModel, SubscriberInfo>();
                // more configs here, if exists
            });
        }
    }

    public static class Extentions
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