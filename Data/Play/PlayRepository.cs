using System;
using System.Globalization;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace dynamodb_test.Data.Play
{
    public class PlayRepository : IPlayRepository<Models.Play>
    {
        private IAmazonDynamoDB DynamoDbClient { get; set; }
        private readonly IDynamoDBContext _context;

        public PlayRepository(IAmazonDynamoDB dynamoDbClient)
        {
            DynamoDbClient = dynamoDbClient;
            _context = new DynamoDBContext(DynamoDbClient);
        }

        public async Task<Models.Play> Save(string player, string game, int score)
        {
            var play = new Models.Play()
            {
                Player = player,
                Game = game,
                Score = score,
                PlayTimeStamp = DateTime.Now.ToString(CultureInfo.CurrentCulture)
            };

            await _context.SaveAsync(play);
            return await RetrieveByHashAndRange(play.Player, play.PlayTimeStamp);
        }

        public async Task<Models.Play> RetrieveByHashAndRange(string hash, string range)
        {
            return await _context.LoadAsync<Models.Play>(hash, range);
        }

        public async Task<Models.Play> Update(Models.Play play, string player, string game, int? score)
        {
            if (!string.IsNullOrEmpty(player)) play.Player = player;
            if (!string.IsNullOrEmpty(game)) play.Game = game;
            if (score != null) play.Score = (int) score;

            await _context.SaveAsync(play);

            return await _context.LoadAsync<Models.Play>(player, play.PlayTimeStamp, new DynamoDBOperationConfig
            {
                ConsistentRead = true
            });
        }

        public async Task<bool> Delete(Models.Play play)
        {
            await _context.DeleteAsync(play);

            await _context.LoadAsync<Models.Play>(play.Player, play.PlayTimeStamp, new DynamoDBOperationConfig
            {
                ConsistentRead = true
            });

            return true;
        }
    }
}