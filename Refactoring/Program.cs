namespace Refactoring
{
    using RefactoringServiceInterface;
    using RefactoringServices;

    public class Program
    {
        public static void Main(string[] args)
        {
            IUserRepository userRepository = new UserRepository(@"Data\Users.json");
            IProductRepository productRepository = new ProductRepository(@"Data\Products.json");
            
            Tusc tusc = new Tusc(userRepository, productRepository);
            tusc.Start();
        }
    }
}
