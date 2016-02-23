namespace Refactoring.View
{
    using RefactoringModel;
    using RefactoringServiceInterface;
    using System;

    public class LoginMenu
    {
        public bool ExitApplication;
        public User User;
        private IUserRepository UserRepository;

        public LoginMenu(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        internal void Show()
        {
            string name = PromptForUserName();
            if (name == null)
            {
                ExitApplication = true;
            }

            User = Login(name);
        }

        private User Login(string name)
        {
            User user = null;

            user = UserRepository.FindUser(name);

            if (user != null)
            {
                string password = PromptForPassword();
                if (user.Password != password)
                {
                    user = null;
                    ShowInvalidPasswordMessage();
                }
            }
            else
            {
                ShowInvalidUserMessage();
            }

            return user;
        }

        private static string PromptForUserName()
        {
            Console.WriteLine();
            Console.WriteLine("Enter Username:");
            string name = Console.ReadLine();
            return name;
        }
        
        private static string PromptForPassword()
        {
            Console.WriteLine("Enter Password:");
            string pwd = Console.ReadLine();
            return pwd;
        }

        private static void ShowInvalidPasswordMessage()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("You entered an invalid password.");
            Console.ResetColor();
        }

        private static void ShowInvalidUserMessage()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("You entered an invalid user.");
            Console.ResetColor();
        }
    }
}
