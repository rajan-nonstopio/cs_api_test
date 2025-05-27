namespace b_api_test;

public static class Config
{
    public static string BaseUrl1 => "https://api-qa.baylorgenetics.com";
    // public static string BaseUrl2 => "https://report-management-api-qa.baylorgenetics.com";
    public static string BaseUrl2 => "https://sample-management-api-qa.baylorgenetics.com";

    private static string token1 => "";

    private static string token2 => "";
    // Report management apis
    private static List<TestApiDetails> reportManagementApis = [
        new TestApiDetails  {
            TestName = "[LIMS] /api/lims/reports/NISC/8691092",
            Method = HttpMethod.Get,
            Endpoint = "/api/lims/reports/NISC/8691092",
        },
        new TestApiDetails
        {
            TestName = "[Reports] Reports by Report {id}",
            Method = HttpMethod.Get,
            Endpoint = "/api/lims/reports/8691092",
        },
        new  TestApiDetails
        {
            TestName ="[Reports] Reports Signature" ,
            Method = HttpMethod.Get,
            Endpoint = "/api/Reports/signature?userId=muh0711%40BaylorGenetics.com&labId=DNA",
        },

        new  TestApiDetails
        {
            TestName ="[Reports] Reports Signature Preview" ,
            Method = HttpMethod.Get,
            Endpoint = "/api/Reports/preview?userId=muh0711%40BaylorGenetics.com&sampleId=8687874",
        },
        new  TestApiDetails
        {
            TestName ="[Reports] Reports templates" ,
            Method = HttpMethod.Get,
            Endpoint = "/api/Reports/templates?templateType=Interpretation",
        }
    ];

    private static List<TestApiDetails> samplemanagement = [
        new  TestApiDetails
        {
            TestName ="[ClientMapping] Get Client Mapping ICD10Code for NISC",
            Method = HttpMethod.Get,
            Endpoint = "/api/ClientMapping?typeName=ICD10Code&hospitalCode=NISC",
        },

        new  TestApiDetails
        {
            TestName ="[clientTestCodes] Get Client Test Codes for NISC with parameters",
            Method = HttpMethod.Get,
            Endpoint = "/api/clientTestCodes?hospitalCode=NISC&name=The Empower™ BRCA1 and BRCA2 Panel(2 genes)&ClientTestCode=NISC-24001-P2-1&testCode=24001&preCarveOutTestCode=Null",
        },
        new  TestApiDetails
        {
            TestName ="[clientTestCodes] Get Client Test Codes for NISC",
            Method = HttpMethod.Get,
            Endpoint = "/api/clientTestCodes/clientcode/NISC-24001-P2-1/hospitalCode/NISC",
        },
        new  TestApiDetails
        {
            TestName ="[clientTestCodes] Get genes for Client Test Code NISC-24001-P2-1",
            Method = HttpMethod.Get,
            Endpoint = "/api/clientTestCodes/clientcode/NISC-24001-P2-1/genes",
        },
        new  TestApiDetails
        {
            TestName ="[clientTestCodes] Get client panel genes for sample 8691206",
            Method = HttpMethod.Get,
            Endpoint = "/api/clientTestCodes/sample/8691206/client-panel-genes",
        },
        new  TestApiDetails
        {
            TestName ="[clientTestCodes] Get client test codes for preCarveOutTestCode 60290",
            Method = HttpMethod.Get,
            Endpoint = "/api/clientTestCodes/preCarveOutTestCode/60290/genes",
        },
        new  TestApiDetails
        {
            TestName ="[clientTestCodes] Get client test codes for labGroupId 7000328",
            Method = HttpMethod.Get,
            Endpoint = "/api/clientTestCodes/labGroupId/7000328/client-panel-genes",
        },        new  TestApiDetails
        {
            TestName ="[clientTestCodes] Get client test codes for labNumber 5009243 and labId DNA",
            Method = HttpMethod.Get,
            Endpoint = "/api/clientTestCodes/labNumber/5009243/labId/DNA/client-panel-genes",
        },
        new  TestApiDetails
        {
            TestName ="[clientTestCodes] Get Client Test Codes with hospitalCode and ClientTestCode",
            Method = HttpMethod.Get,
            Endpoint = "/api/clientTestCodes/code/NISC-24001-P81-1/hospitalCode/NISC",
        },
        new  TestApiDetails
        {
            TestName ="[clientTestCodes] Get Client Test Codes with ClientTestCode",
            Method = HttpMethod.Get,
            Endpoint = "/api/clientTestCodes/code/NISC-24001-P81-1",
        },
        new  TestApiDetails
        {
            TestName ="[clientTestCodes] Get Client Test Codes with ClientTestCode and hospitalCode",
            Method = HttpMethod.Get,
            Endpoint = "/api/clientTestCodes/custom-panel?hospitalCode=NISC&ClientTestCode=60290&testCode=60140&preCarveOutTestCode=60290",
        },
            ///
        new  TestApiDetails
        {
            TestName ="[DnaWgls] Get Lab Number 512521",
            Method = HttpMethod.Get,
            Endpoint = "/api/DnaWgls/LabNumber/512521",
        },
        new  TestApiDetails
        {
            TestName ="[Findings] Get Findings for Sample 8691206",
            Method = HttpMethod.Get,
            Endpoint = "/api/Findings?sampleId=8691206",
        },
        new  TestApiDetails
        {
            TestName ="[Findings] Get Findings for Sample 8691206 with gene CFTR",
            Method = HttpMethod.Get,
            Endpoint = "api/FindingTemplates?gene=CFTR&hospitalCode=NISC",
        },
        new  TestApiDetails
        {
            TestName ="[GeneAware] Get Cases for Sample 8691092",
            Method = HttpMethod.Get,
            Endpoint = "api/GeneAware/Cases/8691092",
        },
        new  TestApiDetails
        {
            TestName ="[GeneAware] Get Findings for Sample 8691092",
            Method = HttpMethod.Get,
            Endpoint = "/api/GeneAware/Cases/8691092/Findings",
        },
        new  TestApiDetails
        {
            TestName ="[GeneAware] Search Get Cases for Sample 8691092 with status InReview and sampleStatus Reported",
            Method = HttpMethod.Get,
            Endpoint = "api/GeneAware/cases/search?sampleIdList=%5B%228691092%22%2C%228685248%22%2C%228226019%22%5D&status=InReview&sampleStatus=Reported&testCodes=60145&top=10&isAscending=true",
        },
        new  TestApiDetails
        {
            TestName ="[Genes] Get Genes AutoSignout Positive Genes",
            Method = HttpMethod.Get,
            Endpoint = "/api/Genes/GeneAware/AutoSignoutPositiveGenes",
        },
        new  TestApiDetails
        {
            TestName ="[Genes] Get Genes ABCC8 Interpretations",
            Method = HttpMethod.Get,
            Endpoint = "/api/Genes/ABCC8/InterpretationsC",
        },
        new  TestApiDetails
        {
            TestName ="[Genes] Get Genes ABCC8 Recommendations",
            Method = HttpMethod.Get,
            Endpoint = "/api/Genes/Recommendations?gene=ABCC8",
        },

        new  TestApiDetails
        {
            TestName ="[Genes] Get Genes CFTR",
            Method = HttpMethod.Get,
            Endpoint = "/api/Genes/search?gene=CFTR",
        },
        new  TestApiDetails
        {
            TestName ="[Genes] Get Genes with alias to standard name mapping",
            Method = HttpMethod.Get,
            Endpoint = "/api/Genes/alias-to-standardName-mapping",
        },

        new  TestApiDetails
        {
            TestName ="[SampleFile] Get Sample File for Sample 8691092",
            Method = HttpMethod.Get,
            Endpoint = "/api/SampleFile/8691092",
        }
    ];



    public static List<TestApiDetails> testSuite => samplemanagement;
    
    
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