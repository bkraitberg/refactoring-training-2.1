namespace RefactoringServices
{
    using Newtonsoft.Json;
    using RefactoringModel;
    using RefactoringServiceInterface;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class UserRepository : IUserRepository
    {
        private string Path;
        private List<User> Users;
        public int Count
        {
            get
            {
                if (Users == null)
                {
                    return 0;
                }
                else
                {
                    return Users.Count;
                }
            }
        }

        public UserRepository(string path)
        {
            Path = path;
            Users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(path));
        }

        public User FindUser(string name)
        {
            return Users.SingleOrDefault(user => user.Name == name);
        }

        public void SaveChanges()
        {
            string json = JsonConvert.SerializeObject(Users, Formatting.Indented);
            File.WriteAllText(Path, json);
        }

        public User[] GetAllUsers()
        {
            return Users.ToArray();
        }
    }
}
