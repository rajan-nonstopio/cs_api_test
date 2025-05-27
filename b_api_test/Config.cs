namespace b_api_test;

public static class Config
{
    public static string BaseUrl1 => "https://api-qa.baylorgenetics.com";
    public static string BaseUrl2 => "https://report-management-api-qa.baylorgenetics.com";

    private static string token1 => "";

    private static string token2 => "";
    
    public static List<TestApiDetails> testSuite => [
      new TestApiDetails  {
            TestName = "LIMS Reports Report by {hospitalCode}/{id}",
            Method = HttpMethod.Get,
            Endpoint = "/api/lims/reports/NISC/8691092",

        },
      new TestApiDetails
      {
          TestName = "LIMS Reports by Report {id}",
          Method = HttpMethod.Get,
          Endpoint = "/api/lims/reports/8691092",
      },
      new  TestApiDetails
      {
          TestName ="LIMS Reports Signature" ,
          Method = HttpMethod.Get,
          Endpoint = "/api/Reports/signature?userId=muh0711%40BaylorGenetics.com&labId=DNA",
      },

      new  TestApiDetails
      {
          TestName ="LIMS Reports Signature Preview" ,
          Method = HttpMethod.Get,
          Endpoint = "/api/Reports/preview?userId=muh0711%40BaylorGenetics.com&sampleId=8687874",
      },
      new  TestApiDetails
      {
          TestName ="LIMS Reports templates" ,
          Method = HttpMethod.Get,
          Endpoint = "/api/Reports/templates?templateType=Interpretation",
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