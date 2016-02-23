namespace RefactoringServiceInterface
{
    using RefactoringModel;

    public interface IUserRepository
    {
        int Count { get; }
        User FindUser(string name);
        void SaveChanges();
        User[] GetAllUsers();
    }
}
