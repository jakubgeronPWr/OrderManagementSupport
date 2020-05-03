﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using OrderManagementSupport.Data.Entities;
using OrderManagementSupport.EntityModel;

namespace OrderManagementSupport.Data
{
    public class OrderManagementMappingProfile: Profile
    {
        public OrderManagementMappingProfile()
        {
            CreateMap<Order, OrderEntityModel>()
                .ForMember(o => o.OrderId, ex => ex.MapFrom(o => o.Id))
                .ForMember(o => o.ClientId, ex => ex.MapFrom( o => o.Client.Id) )
                .ReverseMap();
        }
    }
}