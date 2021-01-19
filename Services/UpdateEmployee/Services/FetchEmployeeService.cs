using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.XRay.Recorder.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Models;

namespace UpdateEmployee.Services
{
    public class FetchEmployeeService: IFetchEmployeeService
    {
        private const string _fetchEmployeeLambdaName = "fetch_employee_nanoservice";

        private readonly AWSXRayRecorder _recorder;
        private readonly ILogger<FetchEmployeeService> _logger;

        public FetchEmployeeService(AWSXRayRecorder recorder, ILogger<FetchEmployeeService> logger) {
            _recorder = recorder;
            _logger = logger;
        }

        public async Task<Employee> ByPersonnelId(string id)
        {
            var request = await CallFetchEmployeeLambda();
            var employee = ExtractEmployee(request.Payload);

            return employee;
        }

        private async Task<InvokeResponse> CallFetchEmployeeLambda() {
            _recorder.BeginSubsegment("CallFetchEmployeeLambda: Making the call");
            var lambdaClient = new AmazonLambdaClient();

            var request = await lambdaClient.InvokeAsync(new InvokeRequest
            {
                FunctionName = "fetch_employee_nanoservice"
            });

            _recorder.EndSubsegment();

            return request;
        }
    
        private Employee ExtractEmployee(MemoryStream payloadStream) {
            _recorder.BeginSubsegment("ExtractEmployee: bytes to employee");
            var asBytes = payloadStream.ToArray();
            var asString = Encoding.UTF8.GetString(asBytes);
            var employee = JsonConvert.DeserializeObject<Employee>(asString);
            _recorder.EndSubsegment();

            return employee;
        }
    }
}
