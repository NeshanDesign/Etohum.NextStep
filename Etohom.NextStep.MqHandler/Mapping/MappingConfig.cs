using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Etohum.NextStep.Common.Dto;
using Etohum.NextStep.Common.Utils;
using Etohum.NextStep.Data.Model;

namespace Etohom.NextStep.MqHandler.Mapping
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            AutoMapper.Mapper.Initialize(cfg => {
                cfg.CreateMap<SubscriberInfo, Subscriber>();
                cfg.CreateMap<Subscriber, SubscriberInfo>();
                // more configs here, if exists
            });
        }
    }
   
}