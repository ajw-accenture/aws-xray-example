using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UpdateEmployee.Services;
using Amazon.XRay.Recorder.Core;
using UpdateEmployee.Bootstrap;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using Amazon.Lambda.Serialization.SystemTextJson;

namespace UpdateEmployee
{
    public class Function
    {
        static Function()
        {
            Initialize();
        }

        static void Initialize()
        {
            AWSSDKHandler.RegisterXRayForAllServices();
        }

        [LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
        public async Task Invoke(object input, ILambdaContext context)
        {
            var provider = DependencyInjection.Initialize();
            var recorder = AWSXRayRecorder.Instance;

            var logger = provider.GetService<ILogger<Function>>();
            var employeeService = provider.GetService<IFetchEmployeeService>();
            var mergeService = provider.GetService<IMergeEmployeeService>();

            var employee = await employeeService.ByPersonnelId("some-employee-id");

            logger.LogInformation($"Fetched employee: {employee.Name} ({employee.PersonnelId})");

            await mergeService.MergeSave(employee);

            logger.LogInformation($"Merge saved employee: {employee.PersonnelId}");
        }
    }
}
