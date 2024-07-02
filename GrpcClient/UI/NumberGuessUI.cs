namespace GrpcClient.UI
{
    public class NumberGuessUI
    {
        private NumberGuess.NumberGuessClient client;
        private PlayerContext playerContext;

        public NumberGuessUI(NumberGuess.NumberGuessClient client)
        {
            this.client = client;
            this.playerContext = new PlayerContext();
        }

        public void Run()
        {
            bool keepPlaying = true;

            SetPlayerContext();

            do
            {
                var desiredAction = GetDesiredActionFromUser();

                switch (desiredAction)
                {
                    case NumberGuessActions.Quit:
                        keepPlaying = false;
                        Console.WriteLine("Thanks for playing!");
                        break;
                    case NumberGuessActions.Reset:
                        PerformAction_Reset();
                        break;
                    case NumberGuessActions.Guess:
                        PerformAction_Guess();
                        break;
                }

                AskUser(" - press enter to continue -");
                Console.Clear();

            } while (keepPlaying);

            Thread.Sleep(2000);
        }


        private void SetPlayerContext()
        {
            var playerName = AskUser("What is your name");
            playerContext.PlayerName = playerName;
            Console.Clear();
        }

        private NumberGuessActions GetDesiredActionFromUser()
        {
            var response = AskUser("" +
                "Menu\r\n" +
                "1.\tReset Game\r\n" +
                "2.\tGuess Number\r\n" +
                "\r\n" +
                "0.\tQuit\r\n" +
                "\r\n" +
                "Please select an action",
                response =>
                    int.TryParse(response, out int result) && Enum.IsDefined((NumberGuessActions)result));

            var selectedAction = (NumberGuessActions)Enum.Parse(typeof(NumberGuessActions), response);

            return selectedAction;
        }

        private void PerformAction_Reset()
        {
            Console.WriteLine("Reseting Game Server...");

            client.ResetNumber(new ResetNumberRequest());

            Console.WriteLine("Game is reset.");
        }

        private void PerformAction_Guess()
        {
            int number = 0;
            var reply = AskUser("Please enter a number", reply => int.TryParse(reply, out number));

            var dto = new NumberGuessRequest
            {
                Name = playerContext.PlayerName,
                Number = number
            };

            var response = client.Guess(dto);

            if (response.Winners.Contains(playerContext.PlayerName))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You are a winner!\r\n");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Sorry, that wasn't it.");
            }

            if (response.Winners.Any())
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("These players are winners:");
                foreach (var winner in response.Winners)
                {
                    Console.WriteLine($" -\t{winner}");
                }
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }

        private string AskUser(string question, Func<string, bool> validate = null)
        {
            bool responseIsValid = false;
            string response = string.Empty;

            do
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(question + ":  ");

                Console.ForegroundColor = ConsoleColor.White;
                response = Console.ReadLine()?.Trim() ?? string.Empty;

                responseIsValid = validate != null ? validate(response) : true;

                if (!responseIsValid)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"'{response}' is invalid.");
                    Console.WriteLine();
                }

            } while (!responseIsValid);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            return response;
        }

        private enum NumberGuessActions
        {
            Quit = 0,
            Reset = 1,
            Guess = 2,
        }
    }
}
