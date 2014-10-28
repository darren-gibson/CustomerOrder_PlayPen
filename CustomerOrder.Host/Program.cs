namespace CustomerOrder.Host
{
    using System;
    using System.Collections.Generic;
    using Model;
    using Nancy.Hosting.Self;
    using PriceServiceStub;
    using ProductServiceStub;

    class Program
    {
        static void Main(string[] args)
        {
            var uri = new Uri("http://localhost:3579");

            var bootstrapper = new Bootstrapper();
            using (var host = new NancyHost(uri, bootstrapper))
            {
                host.Start();

                LoadProducts(bootstrapper.Registry);

                Console.WriteLine("Your application is running on " + uri);
                Console.WriteLine("Press any [Enter] to close the host.");
                Console.ReadLine();
            }
        }

        private static void LoadProducts(Registry registry)
        {
            var priceGateway = registry.Resolve<SimplePriceGateway>();
            var productService = registry.Resolve<SimpleProductService>();
            foreach (var product in GetProducts())
            {
                priceGateway.SetPrice(product.ProductIdentifier, product.UnitPrice);
                productService.AddProduct(product.ProductIdentifier.ToString(), product.Description, product.ImageUrl);
            }
        }

        private static IEnumerable<Product> GetProducts()
        {
            return new[]
            {
                new Product(55, "Tesco Lemons Each", 0.35m, UnitOfMeasure.Each, "http://img.tesco.com/Groceries/pi/000/0223780000000/IDShot_110x110.jpg"),
                new Product(5018374888303, "Tesco Free Range Eggs Large Box Of 6", 1.30m, UnitOfMeasure.Each, "http://img.tesco.com/Groceries/pi/303/5018374888303/IDShot_110x110.jpg"),
                new Product(0296220000000, "Counter Whole Sea Bream 300G", 3.00m, UnitOfMeasure.Each, "http://img.tesco.com/Groceries/pi/000/0296220000000/IDShot_110x110.jpg"),
                new Product(0266990000000, "Tesco Braeburn Apples Loose", 1.95m, UnitOfMeasure.KG, "http://img.tesco.com/Groceries/pi/000/0266990000000/IDShot_110x110.jpg"),
                new Product(5000157024671, "Heinz Baked Beans In Tomato Sauce 415G", 0.68m, UnitOfMeasure.Each, "http://img.tesco.com/Groceries/pi/671/5000157024671/IDShot_110x110.jpg"),
            };
        }

        internal class Product
        {
            public ProductIdentifier ProductIdentifier { get; private set; }
            public string Description { get; private set; }
            public QuantityPrice UnitPrice { get; private set; }
            public string ImageUrl { get; private set; }

            public Product(ProductIdentifier productIdentifier, string description, decimal unitPrice, UnitOfMeasure sellByUnitOfMeasure, string imageUrl)
            {
                ProductIdentifier = productIdentifier;
                Description = description;
                UnitPrice = new QuantityPrice(new Money(Currency.GBP, unitPrice), new Quantity(1, sellByUnitOfMeasure));
                ImageUrl = imageUrl;
            }
        }
    }
}
