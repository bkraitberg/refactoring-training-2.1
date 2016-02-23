namespace Refactoring.View
{
    using RefactoringModel;
    using RefactoringServiceInterface;
    using System;

    public class ProductMenu
    {
        public bool ExitApplication;
        public Product SelectedProduct;

        private IProductRepository ProductRepository;

        public ProductMenu(IProductRepository productRepository)
        {
            ProductRepository = productRepository;
        }

        public void Show()
        {
            ShowProducts();
            int productId = PromptForDesiredProductId();

            if (productId == ProductRepository.Count)
            {
                SelectedProduct = null;
                ExitApplication = true;
            }
            else
            {
                SelectedProduct = ProductRepository.FindByIndex(productId);
            }
        }
        
        private void ShowProducts()
        {
            // Prompt for user input
            Console.WriteLine();
            Console.WriteLine("What would you like to buy?");

            Product[] products = ProductRepository.GetAllProducts();
            for (int i = 0; i < products.Length; ++i)
            {
                Product product = products[i];
                Console.WriteLine(i + 1 + ": " + product.Name + " (" + product.Price.ToString("C") + ")");
            }
            Console.WriteLine(ProductRepository.Count + 1 + ": Exit");
        }

        private static int PromptForDesiredProductId()
        {
            // Prompt for user input
            Console.WriteLine("Enter a number:");
            string answer = Console.ReadLine();
            int num = Convert.ToInt32(answer);
            num = num - 1;
            return num;
        }
    }
}
