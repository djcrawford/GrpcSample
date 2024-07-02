using GrpcService;

namespace UnitTests
{
    public class Tests
    {
        public GameContext GameContext { get; set; }
        private const string Player1Name = "Player1";

        [SetUp]
        public void Setup()
        {
            GameContext = new GameContext();
        }

        #region Reset()

        [Test]
        public void Reset_ClearsPlayerList()
        {
            GameContext.Players.Add(Player1Name);

            GameContext.Reset();

            Assert.IsEmpty(GameContext.Players);
        }

        [Test]
        public void Reset_ClearsWinnerList()
        {
            GameContext.Winners.Add(Player1Name);

            GameContext.Reset();

            Assert.IsEmpty(GameContext.Winners);
        }

        [Test]
        public void Reset_ChangesWinningNumber()
        {
            var original = GameContext.WinningNumber;

            for (var i = 0; i < 5; i++)
            {
                GameContext.Reset();

                if (original != GameContext.WinningNumber)
                {
                    Assert.Pass();
                }
            }

            Assert.Fail();
        }

        #endregion Reset()

        #region Guess()

        [Test]
        public void Guess_AddsUserToPlayers()
        {
            GameContext.Guess(Player1Name, 1);

            Assert.Contains(Player1Name, GameContext.Players.ToList());
        }

        [Test]
        public void Guess_WinningPlayerAddedToWinners()
        {
            GameContext.WinningNumber = 1;
            GameContext.Guess(Player1Name, 1);

            Assert.That(GameContext.Players.Where(pn => pn == Player1Name).Count() == 1);
        }

        [Test]
        public void Guess_MultipleGuessesFromPlayer1_PlayerNotDuplicatedInPlayers()
        {
            GameContext.Guess(Player1Name, 1);
            GameContext.Guess(Player1Name, 2);

            Assert.That(GameContext.Players.Where(pn => pn == Player1Name).Count() == 1);
        }

        [Test]
        public void Guess_MultipleWinningGuessesFromPlayer1_Player1NotDuplicatedInWinners()
        {
            GameContext.WinningNumber = 1;
            GameContext.Guess(Player1Name, 1);
            GameContext.Guess(Player1Name, 1);

            Assert.That(GameContext.Players.Where(pn => pn == Player1Name).Count() == 1);
        }

        #endregion Guess()
    }
}