using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

        [Amazon.Lambda.Core.LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
        public async Task Invoke(object input, ILambdaContext context)
        {
            var container = Container.Initialize();
            var logger = container.GetService<ILogger<Function>>();
            var employeeService = container.GetService<IFetchEmployeeService>();

            var employee = await employeeService.ByPersonnelId("some-employee-id");

            logger.LogInformation($"Fetched employee: {employee.Name} ({employee.PersonnelId})");
        }
    }
}
