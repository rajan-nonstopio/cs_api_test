public class TestApiDetails
{
    public string Endpoint { get; set; }
    public HttpMethod Method { get; set; }
    public object? Data { get; set; }
    public string TestName { get; set; }
}

public static class GeneratedTestApiDetails
{
    public static List<TestApiDetails> GetTestApiDetailsList()
    {
        return
        [
            new TestApiDetails
            {
                Endpoint = "/api/lims/reports/{hospitalCode}/{id}",
                Method = HttpMethod.Get,
                Data = null,
                TestName =
                    "GET_lims_reports_hospitalCode_id_Gets_a_report_with_id_and_hospital_code"
            },

            new TestApiDetails
            {
                Endpoint = "/api/lims/reports/{id}",
                Method = HttpMethod.Get,
                Data = null,
                TestName = "GET_lims_reports_id_Gets_a_report_with_id"
            },

            new TestApiDetails
            {
                Endpoint = "/api/Reports/generate-reports-async",
                Method = HttpMethod.Post,
                Data = null,
                TestName =
                    "POST_Reports_generate-reports-async_generate_batch_reports_asynchronously"
            },

            new TestApiDetails
            {
                Endpoint = "/api/Reports/signout",
                Method = HttpMethod.Post,
                Data = null,
                TestName = "POST_Reports_signout_generate_batch_sign_out_reports (Deprecated)"
            },

            new TestApiDetails
            {
                Endpoint = "/api/Reports/signout-async",
                Method = HttpMethod.Post,
                Data = null,
                TestName = "POST_Reports_signout-async_sign_out_batch_samples_asynchronously"
            },

            new TestApiDetails
            {
                Endpoint = "/api/Reports/signature",
                Method = HttpMethod.Get,
                Data = null,
                TestName = "GET_Reports_signature_Check_to_see_if_signature_is_valid"
            },

            new TestApiDetails
            {
                Endpoint = "/api/Reports",
                Method = HttpMethod.Post,
                Data =
                    @"{""caseId"":""CASE123"",""sampleId"":1001,""sampleName"":""Sample_Test_001""}",
                TestName = "POST_Reports_Generate_Report"
            },

            new TestApiDetails
            {
                Endpoint = "/api/Reports",
                Method = HttpMethod.Get,
                Data = null,
                TestName = "GET_Reports_Get_all_Reports."
            },

            new TestApiDetails
            {
                Endpoint = "/api/Reports/reporttemplates",
                Method = HttpMethod.Get,
                Data = null,
                TestName = "GET_Reports_reporttemplates_Gets_all_report_templates"
            },

            new TestApiDetails
            {
                Endpoint = "/api/Reports/preview",
                Method = HttpMethod.Get,
                Data = null,
                TestName = "GET_Reports_preview_Gets_preview_of_report"
            },

            new TestApiDetails
            {
                Endpoint = "/api/Reports/templates",
                Method = HttpMethod.Get,
                Data = null,
                TestName = "GET_Reports_templates_Get_Template_Options_for_a_Report"
            },

            new TestApiDetails
            {
                Endpoint = "/api/Reports2/{sampleId}/Preview",
                Method = HttpMethod.Post,
                Data = "sample_string_data",
                TestName = "POST_Reports2_sampleId_Preview"
            },

            new TestApiDetails
            {
                Endpoint = "/api/Reports2/{sampleId}/Signout",
                Method = HttpMethod.Post,
                Data = "sample_string_data",
                TestName = "POST_Reports2_sampleId_Signout"
            }

        ];
    }
}