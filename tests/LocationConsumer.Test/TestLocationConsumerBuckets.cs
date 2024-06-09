

using Cassandra;
using LocationConsumer.Helper;
using LocationConsumer.Repo;
using Moq;

namespace LocationConsumer.Test;


public class TestLocationConsumerBuckets
{


  [Theory]
  [InlineData("2024-01-01T00:00:00Z", "1-2024")]
  [InlineData("2024-06-08T00:00:00Z", "23-2024")]
  [InlineData("2023-12-31T00:00:00Z", "52-2023")]
  [InlineData("2025-05-19T00:00:00Z", "21-2025")]
  [InlineData("2024-12-31T23:59:59Z", "1-2025")] 
  public void BucketWeekYearTest(string timestampString, string expectedBucket)
  {

    //arrange
    var timestamp = DateTime.Parse(timestampString);
    
    //act
    string actualBucket = BucketCalculation.BucketWeekYear(timestamp);
    
    //assert
    Assert.Equal(expectedBucket, actualBucket);
  }

  [Theory]
  [InlineData("2024-01-01T00:00:00", "00-01-01-2024")]
  [InlineData("2024-12-31T23:59:59", "23-31-12-2024")]
  public void BucketHourDateTest(string timestampString, string expectedBucket)
  {
    //arrange
    var timestamp = DateTime.Parse(timestampString);

    //act
    string actualBucket = BucketCalculation.BucketHourDate(timestamp);
      
    //assert
    Assert.Equal(expectedBucket, actualBucket);
    }
  }