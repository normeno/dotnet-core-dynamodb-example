using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using dynamodb_test.Data.Play;
using Microsoft.AspNetCore.Mvc;

namespace dynamodb_test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayController : ControllerBase
    {
        private IAmazonDynamoDB DynamoDbClient { get; }
        
        // Valores para usar de prueba
        private readonly List<string> _games = new List<string> {"The Legend of Zelda", "Final Fantasy"};
        private readonly List<string> _players = new List<string> {"Link", "Skull Kid", "Squall Leonhart", "Artemisa"};

        public PlayController(IAmazonDynamoDB dynamoDbClient)
        {
            DynamoDbClient = dynamoDbClient;
        }

        [HttpGet]
        public async Task<Hashtable> Get()
        {
            var response = new Hashtable();

            try
            {
                var random = new Random();
                var playRepository = new PlayRepository(DynamoDbClient);

                var newPlay = await playRepository.Save(_players[random.Next(_players.Count)],
                    _games[random.Next(_games.Count)], random.Next(0, 100));

                response["newPlay"] = newPlay;

                var updatePlay = await playRepository.Update(newPlay, "modificado", null, null);

                response["updatePlay"] = updatePlay;

                var getBeforeDelete =
                    await playRepository.RetrieveByHashAndRange(updatePlay.Player, updatePlay.PlayTimeStamp);

                response["getBeforeDelete"] = getBeforeDelete;

                var deleted = await playRepository.Delete(updatePlay);

                response["deleted"] = deleted;

                var getAfterDelete =
                    await playRepository.RetrieveByHashAndRange(updatePlay.Player, updatePlay.PlayTimeStamp);

                response["getAfterDelete"] = getAfterDelete;

                return response;
            }
            catch (Exception e)
            {
                response["error"] = e.Message;
            }

            return response;
        }
    }
}