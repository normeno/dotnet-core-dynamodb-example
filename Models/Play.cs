using Amazon.DynamoDBv2.DataModel;

namespace dynamodb_test.Models
{
    [DynamoDBTable("Plays")]
    public class Play
    {
        [DynamoDBHashKey] public string Player { get; set; }

        [DynamoDBRangeKey] public string PlayTimeStamp { get; set; }

        [DynamoDBProperty] public string Game { get; set; }

        [DynamoDBProperty] public int Score { get; set; }
    }
}