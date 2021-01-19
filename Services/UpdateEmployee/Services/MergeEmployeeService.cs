using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.XRay.Recorder.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Models;

namespace UpdateEmployee.Services
{
    public class MergeEmployeeService : IMergeEmployeeService
    {
        private const string mergeEmployeeLambdaName = "merge_employee_nanoservice";
        private readonly AWSXRayRecorder _recorder;
        private readonly ILogger<MergeEmployeeService> _logger;

        public MergeEmployeeService(AWSXRayRecorder recorder, ILogger<MergeEmployeeService> logger)
        {
            _recorder = recorder;
            _logger = logger;
        }

        public async Task MergeSave(Employee employee)
        {
            _recorder.BeginSubsegment("SEG Serialize employee object");
            var employeeSerialized = JsonConvert.SerializeObject(employee);
            _recorder.EndSubsegment();

            var lambdaClient = new AmazonLambdaClient();

            var request = await lambdaClient.InvokeAsync(new InvokeRequest
            {
                FunctionName = mergeEmployeeLambdaName,
                Payload = employeeSerialized
            });
        }
    }
}
