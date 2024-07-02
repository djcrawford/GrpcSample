namespace GrpcService
{
    public class GameContext
    {
        public const int MinWinningNumber = 0;
        public const int MaxWinningNumber = 10;

        public int WinningNumber { get; set; }
        public HashSet<string> Winners { get; set; } = new HashSet<string>();

        private Random random = new Random();

        public GameContext()
        {
            Reset();
        }

        public void Reset()
        {
            Winners.Clear();
            WinningNumber = random.Next(MinWinningNumber, MaxWinningNumber + 1);
        }

        public void Guess(string playerName, int guess)
        {
            if (guess == WinningNumber)
            {
                Winners.Add(playerName);
            }
        }
    }
}
