using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UpdateEmployee.Services;
using Amazon.XRay.Recorder.Core;
using UpdateEmployee.Bootstrap;

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
            var recorder = new AWSXRayRecorderBuilder().Build();
            AWSXRayRecorder.InitializeInstance(null, recorder);
        }

        [Amazon.Lambda.Core.LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
        public async Task Invoke(object input, ILambdaContext context)
        {
            var provider = DependencyInjection.Initialize();
            var recorder = AWSXRayRecorder.Instance;

            var logger = provider.GetService<ILogger<Function>>();
            var employeeService = provider.GetService<IFetchEmployeeService>();

            var employee = await employeeService.ByPersonnelId("some-employee-id");

            logger.LogInformation($"Fetched employee: {employee.Name} ({employee.PersonnelId})");
        }
    }
}
