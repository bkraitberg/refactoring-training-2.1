namespace RefactoringServices
{
    using Newtonsoft.Json;
    using RefactoringModel;
    using RefactoringServiceInterface;
    using System.Collections.Generic;
    using System.IO;

    public class ProductRepository : IProductRepository
    {
        private string Path;
        private List<Product> Products;
        public int Count
        {
            get
            {
                if (Products == null)
                {
                    return 0;
                }
                else
                {
                    return Products.Count;
                }
            }
        }

        public ProductRepository(string path)
        {
            Path = path;
            Products = JsonConvert.DeserializeObject<List<Product>>(File.ReadAllText(path));
        }

        public void SaveChanges()
        {
            string json = JsonConvert.SerializeObject(Products, Formatting.Indented);
            File.WriteAllText(Path, json);
        }

        public Product FindByIndex(int index)
        {
            return Products[index];
        }

        public Product[] GetAllProducts()
        {
            return Products.ToArray();
        }
    }
}
