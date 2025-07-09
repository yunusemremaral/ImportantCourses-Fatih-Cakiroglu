using RedisExampleApp.API.Models;
using RedisExampleApp.Cache;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExampleApp.API.Repository
{
    public class ProductRepositoryWithCacheDecorator : IProductRepository
    {
        private const string productkey = "productCaches";
        private readonly IProductRepository _productRepository;
        private readonly RedisService _redisService;
        private readonly IDatabase _cacherepository;

        public ProductRepositoryWithCacheDecorator(IProductRepository productRepository, RedisService redisService
            )
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _cacherepository = _redisService.GetDb(2);

        }

        public async Task<Product> CreateAsync(Product product)
        {
            var newProduct = await _productRepository.CreateAsync(product);

            if (await _cacherepository.KeyExistsAsync(productkey))
            {
                await _cacherepository.HashSetAsync(productkey, newProduct.Id, JsonSerializer.Serialize(newProduct));
            }

            return newProduct;
        }


        public async Task<List<Product>> GetAsync()
        {
            if (!await _cacherepository.KeyExistsAsync(productkey))
                return await LoadToCacheFromDbAsync();

            var products = new List<Product>();
            var cacheProducts = await _cacherepository.HashGetAllAsync(productkey);

            foreach (var item in cacheProducts)
            {
                var product = JsonSerializer.Deserialize<Product>(item.Value);
                products.Add(product);
            }

            return products;
        }


        public async Task<Product> GetByIdAsync(int id)
        {
            if (_cacherepository.KeyExists(productkey))
            {
                var product = await _cacherepository.HashGetAsync(productkey, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            }

            var products = await LoadToCacheFromDbAsync();
            return products.FirstOrDefault(x => x.Id == id);
        }


        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var products = await _productRepository.GetAsync();

            products.ForEach(p =>
            {
            _cacherepository.HashSet(productkey, p.Id, JsonSerializer.Serialize(p));
            });
            return products;
        }
    }
}
