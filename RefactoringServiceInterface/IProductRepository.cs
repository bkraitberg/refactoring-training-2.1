namespace RefactoringServiceInterface
{
    using RefactoringModel;

    public interface IProductRepository
    {
        int Count { get; }
        void SaveChanges();
        Product FindByIndex(int index);
        Product[] GetAllProducts();
    }
}
