﻿namespace CustomerOrder.Contract.DTO
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;
    using Newtonsoft.Json;

    public class CustomerOrder
    {
        private readonly ICustomerOrder _order;

        public CustomerOrder(ICustomerOrder order)
        {
            _order = order;
        }

        [JsonProperty(PropertyName = "products")]
        public IEnumerable<Product> Products { get { return _order.Products.Select(p => new Product(p, _order)); } }
        [JsonProperty(PropertyName = "total")]
        public OrderTotal Total { get { return new OrderTotal(_order); } }

    }
}