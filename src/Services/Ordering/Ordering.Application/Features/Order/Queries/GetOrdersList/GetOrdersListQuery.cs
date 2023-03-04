﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Order.Queries.GetOrdersList
{
    public class GetOrdersListQuery:IRequest<List<OrdersVM>>
    {
        public string UserName { get; set; }
        public GetOrdersListQuery(string UserName)
        {
            this.UserName = UserName;
        }
    }
}
