using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace Fruktsallad
{
    class Program
    {
        static Dictionary<string, double> shoppingList = new Dictionary<string, double>();
        static bool moreItems = true;
        static int storFruktsallad = 0;

        
        static void Main(string[] args)
        {

            StartProgram();
            StartShopping();

        }

        private static void StartProgram()
        {
            string versionNumber = "1.0.0";
            string author = "Mattias Berglund";
            int copyrightYear = 2019;
            string programName = "\"Inköpslista till fruktsallad\"";

            Console.Write("Välkommen till ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(programName + ".");
            Console.ResetColor();
            Console.WriteLine($"© {copyrightYear} {author}, Version: {versionNumber}\n");

            Console.WriteLine("Tryck på enter för att starta programmet.");
            Console.ReadLine();
        }

        private static void StartShopping()
        {
            bool anotherList = true;

            // Keep creating new shopping lists until user is done.
            while (anotherList)
            {
                try
                {
                    CreateShoppingList();
                    ReturnItems();
                    Console.Clear();
                    anotherList = GetYesOrNoInput("Vill du skapa en till inköpslista? [ja/nej]: ");
                    if (anotherList)
                    {
                        if (!GetYesOrNoInput("Vill du fortsätta på den förra inköpslistan? "))
                        {
                            shoppingList.Clear(); // If user wants to create a new blank list.
                        }
                        storFruktsallad = 0;
                    }
                    Console.Clear();
                }
                catch (Exception)
                {
                    Console.WriteLine("Hoppsan! Någonting oväntat gick fel, programmet avslutas.");
                    anotherList = false;
                    Thread.Sleep(2500);
                }
            }
        }

        private static void CreateShoppingList()
        {
            bool userInput = true;

            Console.Clear();


            switch (userInput)
            {
                case true:
                    moreItems = true;
                    break;
                case false:
                    moreItems = false;
                    break;
            }

            while (moreItems) // Keep adding items to shoppinglist as long as user wants to.
            {
                AddItem();
                Console.Clear();
            }
        }

        private static void AddItem()
        {
            double itemPrice = 0;
            string itemName = "";
            bool validItemName = false;
            
            while (!validItemName)
            {
                Console.WriteLine("Skriv \"cmd\" för att se alla kommandon eller ange varans namn.");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Ange varans namn: ");
                Console.ResetColor();
                itemName = Console.ReadLine().Trim().ToLower();

                switch (itemName)
                {
                    case "cmd":
                        ShowCommands();
                        break;
                    case "visa lista":
                        ShowShoppingList();
                        break;
                    default:
                        if (shoppingList.ContainsKey(itemName))
                        {
                            string removeDuplicateMessage = $"Du har redan lagt till {itemName}. Vill du ta " +
                                $"bort varan från inköpslistan? (ja/nej): ";
                            if (GetYesOrNoInput(removeDuplicateMessage))
                            {
                                shoppingList.Remove(itemName);
                                Console.WriteLine($"{itemName} har tagits bort från inköpslistan.");
                                Thread.Sleep(1200);
                            }
                            Console.Clear();
                        }
                        else
                        {
                            validItemName = true;
                        }
                        break;
                }
            }

            if (itemName != "")
            {
                bool validInput = false;

                while (!validInput)
                {
                    Console.Write($"Vad kostar {itemName}? (kr): ");

                    if (double.TryParse(Console.ReadLine().Trim().ToLower(), out itemPrice))
                    {
                        if (itemPrice < 0)
                        {
                            string message = $"Är du säker på att du får betalt för att köpa {itemName}? ";
                            validInput = GetYesOrNoInput(message);
                            if (validInput)
                            {
                                Console.WriteLine("Okej då...");
                            }
                        }
                        else if (itemPrice == 0)
                        {
                            string message = $"Är du säker på att {itemName} är gratis? ";
                            validInput = GetYesOrNoInput(message);
                            if (validInput)
                            {
                                Console.WriteLine("Där skulle jag också vilja handla...");
                                Thread.Sleep(1000);
                            }
                        }
                        else if (itemPrice > 300)
                        {
                            string message = $"Är du säker på att {itemName} kostar så mycket? [ja/nej] ";
                            validInput = GetYesOrNoInput(message);
                            if (validInput && storFruktsallad == 0)
                            {
                                Console.WriteLine("Antingen så betalar du kraftigt överpris, eller så skall " +
                                    "det bli en väldigt stor fruktsallad. Oavsett vilket så är jag glad över att det " +
                                    "inte är jag som betalar.\n");
                                storFruktsallad++;
                                Thread.Sleep(1000);
                                Console.Write("Tryck enter för att fortsätta...");
                                Console.ReadLine();
                            }
                        }
                        else
                        {
                            validInput = true;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ogiltig input. Exempel på giltig input: 65,50 eller 73");
                        Console.ResetColor();
                    }
                }
                shoppingList.Add(itemName, itemPrice);
                Console.WriteLine($"{itemName} för {itemPrice} kr tillagt.");
                Thread.Sleep(800);
            }
            else // If user doesn't type anything, exit program.
            {
                moreItems = false;
            }
        }

        private static void ShowShoppingList()
        {
            if (shoppingList.Count() == 0)
            {
                Console.WriteLine("Din inköpslista är tom.\n");
                Console.WriteLine("Tryck på enter för att fortsätta...");
                Console.ReadLine();
                Console.Clear();
            }
            else
            {
                foreach (KeyValuePair<string, double> keyValue in shoppingList)
                {
                    Console.WriteLine($"{keyValue.Key}: {keyValue.Value} kr");
                }
                Console.WriteLine();
                Console.Write("Tryck på enter för att fortsätta...");
                Console.ReadLine();
                Console.Clear();
            }
        }

        private static void ShowCommands()
        {
            Console.Clear();
            Console.WriteLine("Möjliga kommandon: " +
                "\t1. Lämna tomt - Färdig med inmatning, visa dyraste och billigaste varorna." +
                "\n\t\t\t2. \"Visa lista\": Visa shoppinglistan." +
                "\n\t\t\t3. \"cmd\": Visa tillgängliga kommandon." + 
                "\n\t\t\t4. Om du vill ta bort en vara, skriv in namnet på varan igen." +
                "\n\t\t\t5. Annars skriver du bara in varans namn för att lägga till i inköpslistan.\n");
        }

        private static bool GetYesOrNoInput(string inputMessage)
        {
            // Input: Message to print before asking user to write "ja" or "nej".

            // Returns: True for "ja" and False for "nej".


            string[] validInput = { "ja", "nej" };
            string userInput = null;

            while (!validInput.Contains(userInput)) // Check that user types a valid input (ja/nej)
            {
                Console.Write(inputMessage);
                userInput = Console.ReadLine().Trim().ToLower();

                if (!validInput.Contains(userInput))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ogiltig input. Skriv \"ja\" eller \"nej\".");
                    Console.ResetColor();
                }
            }

            switch (userInput)
            {
                case "ja":
                    return true;
                case "nej":
                    return false;
                default:
                    return false; // This will never happen since input must be ja/nej.
            }
        }

        private static void ReturnItems()
        {      
            try
            {
                // Get maximum value from dictionary.
                var maxKey = shoppingList.Values.Max(); 
                var keyOfMaxValue = shoppingList.Aggregate((x, y) => 
                    x.Value > y.Value ? x : y).Key;

                // Get minimum value from dictionary.
                var minKey = shoppingList.Values.Min();
                var keyOfMinValue = shoppingList.Aggregate((x, y) =>
                    x.Value < y.Value ? x : y).Key;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Den dyraste produkten i listan är {keyOfMaxValue}. Den " +
                    $"kostar {maxKey} kronor.");
                Console.WriteLine($"Den billigaste produkten i listan är {keyOfMinValue}. Den " +
                    $"kostar {minKey} kronor.");
                Console.ResetColor();

                Console.WriteLine();
                Console.Write("Tryck på enter för att fortsätta...");
                Console.ReadLine();
            }
            catch (Exception)
            {
                if (shoppingList.Count == 0)
                {
                    Console.WriteLine("Shoppinglist was empty");
                    Thread.Sleep(1500);
                }
                else
                {
                    Console.WriteLine("Something unexpected happened...");
                }
            }
        }

    }
}
