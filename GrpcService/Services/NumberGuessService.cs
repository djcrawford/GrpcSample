using Grpc.Core;

namespace GrpcService.Services
{
    public class NumberGuessService : NumberGuess.NumberGuessBase
    {
        protected GameContext _gameContext { get; set; }

        public NumberGuessService(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public override Task<ResetNumberReply> ResetNumber(ResetNumberRequest request, ServerCallContext context)
        {
            _gameContext.Reset();

            var reply = new ResetNumberReply
            {
                RangeStart = GameContext.MinWinningNumber,
                RangeEnd = GameContext.MaxWinningNumber
            };

            return Task.FromResult(reply);
        }

        public override Task<NumberGuessReply> Guess(NumberGuessRequest request, ServerCallContext context)
        {
            _gameContext.Guess(request.Name, request.Number);

            var reply = new NumberGuessReply();
            reply.Winners.AddRange(_gameContext.Winners);

            return Task.FromResult(reply);
        }
    }
}
