namespace b_api_test;

public static class Config
{
    public static string BaseUrl1 => "https://api-qa.baylorgenetics.com";
    public static string BaseUrl2 => "https://sample-management-api-qa.baylorgenetics.com";

    private static string token1 => "";

    private static string token2 => "";
    
    public static List<TestApiDetails> testSuite => [
      new TestApiDetails  {
            TestName = "Client Custom panels",
            Method = HttpMethod.Get,
            Endpoint = "/api/clientTestCodes/custom-panel?hospitalCode=NISC&preCarveOutTestCode=24001",
          
        },
      new TestApiDetails
      {
          TestName = "Client Test code",
          Method = HttpMethod.Get,
          Endpoint = "/api/clientTestCodes/code/24001",
      },
      new  TestApiDetails
      {
          TestName ="Get Client Test codes" ,
          Method = HttpMethod.Get,
          Endpoint = "/api/clientTestCodes?clientTestcode=NISC-24001-P2-1",
      },
      new TestApiDetails {
        TestName = "Create client test code",
        Method = HttpMethod.Post,
        Endpoint = "api/clientTestCodes",
        Data =  new
        {
            name = "ANISC-24001-P2-17761",
            hospitalCode = "NISC", 
            reportName = "POST TEST API name",
            processorName = "GANateraProcessor",
            testCode = 24001,
            billingCode = "PRPN",
            genes = new[]
            {
                "EPCAM",
                "MLH1"
            }
        }
      }
    ];
    
    
    public static ApiTester CreateDefaultTester()
    {
        return new ApiTester(
            // Configure the first API service (Service A)
            serviceA => serviceA
                .SetBaseUrl(BaseUrl1)
                .AddHeader("accept", "application/json"),
            
            // Configure a second API service (Service B)
            serviceB => serviceB
                .SetBaseUrl(BaseUrl2)
                .AddHeader("accept", "application/json")
        );
    }
}

public class TestApiDetails
{
    public string Endpoint { get; set; }
    public HttpMethod Method { get; set; }
    public object? Data { get; set; }
    public string TestName { get; set; }
}