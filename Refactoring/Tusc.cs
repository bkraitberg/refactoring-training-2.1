namespace Refactoring
{
    using Refactoring.View;
    using RefactoringModel;
    using RefactoringServiceInterface;
    using System;

    public class Tusc
    {
        private IUserRepository UserRepository { get; set; }
        private IProductRepository ProductRepository { get; set; }

        public Tusc(IUserRepository userRepository, IProductRepository productRepository)
        {
            UserRepository = userRepository;
            ProductRepository = productRepository;
        }

        public void Start()
        {
            bool exiting = false;
            ShowWelcomeMessage();

            while (!exiting)
            {
                LoginMenu loginMenu = new LoginMenu(UserRepository);
                loginMenu.Show();

                if (loginMenu.ExitApplication)
                {
                    exiting = true;
                }
                else
                {
                    if (loginMenu.User != null)
                    {
                        ShowWelcomeMessage(loginMenu.User.Name);
                        Console.WriteLine();
                        ShowBalance(loginMenu.User.Balance);

                        // Show product list
                        while (!exiting)
                        {
                            ProductMenu productMenu = new ProductMenu(ProductRepository);
                            productMenu.Show();

                            if (productMenu.ExitApplication)
                            {
                                exiting = true;
                            }
                            else
                            {
                                Console.WriteLine();
                                ShowProductName(productMenu.SelectedProduct);
                                ShowBalance(loginMenu.User.Balance);

                                int quantity = PromptForQuantity();
                                var totalPrice = productMenu.SelectedProduct.Price * quantity;

                                if (loginMenu.User.Balance - totalPrice < 0)
                                {
                                    ShowNotEnoughMoneyMessage();
                                    continue;
                                }

                                if (productMenu.SelectedProduct.Quantity <= quantity)
                                {
                                    ShowNotEnoughProductMessage(productMenu);
                                    continue;
                                }

                                if (quantity > 0)
                                {
                                    loginMenu.User.Balance -= productMenu.SelectedProduct.Price * quantity;
                                    productMenu.SelectedProduct.Quantity -= quantity;

                                    Console.Clear();
                                    ShowPurchaseDetails(quantity, productMenu.SelectedProduct.Name, loginMenu.User.Balance);
                                }
                                else
                                {
                                    // Quantity is less than zero
                                    Console.Clear();
                                    ShowCancelledPurchaseMessage();
                                }
                            }
                        }
                    }
                }
            }

            // Prevent console from closing
            Console.WriteLine();
            Console.WriteLine("Press Enter key to exit");
            Console.ReadLine();
        }

        #region Console Helpers

        private static void ShowCancelledPurchaseMessage()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("Purchase cancelled");
            Console.ResetColor();
        }

        private static void ShowPurchaseDetails(int quantity, string productName, double newBalance)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("You bought " + quantity + " " + productName);
            Console.WriteLine("Your new balance is " + newBalance.ToString("C"));
            Console.ResetColor();
        }

        private static void ShowNotEnoughProductMessage(ProductMenu productMenu)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("Sorry, " + productMenu.SelectedProduct.Name + " is out of stock");
            Console.ResetColor();
        }

        private static void ShowNotEnoughMoneyMessage()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("You do not have enough money to buy that.");
            Console.ResetColor();
        }

        private static int PromptForQuantity()
        {
            // Prompt for user input
            Console.WriteLine("Enter amount to purchase:");

            string answer2 = Console.ReadLine();
            int qty = Convert.ToInt32(answer2);
            return qty;
        }

        private static void ShowProductName(Product product)
        {
            Console.WriteLine("You want to buy: " + product.Name);
        }

        private static void ConfirmClose()
        {
            // Prevent console from closing
            Console.WriteLine();
            Console.WriteLine("Press Enter key to exit");
            Console.ReadLine();
        }

        private static void ShowBalance(double bal)
        {
            Console.WriteLine("Your balance is " + bal.ToString("C"));
        }

        private static void ShowWelcomeMessage(string name)
        {
            // Show welcome message
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("Login successful! Welcome " + name + "!");
            Console.ResetColor();
        }

        private bool IsValidPassword(User user, string pwd)
        {
            return user.Password.Equals(pwd);
        }

        private static void ShowWelcomeMessage()
        {
            // Write welcome message
            Console.WriteLine("Welcome to TUSC");
            Console.WriteLine("---------------");
        }

        #endregion

    }
}
