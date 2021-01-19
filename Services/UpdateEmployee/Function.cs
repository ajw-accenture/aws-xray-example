using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Lambda.Core;
using Amazon.Lambda.Model;
using Amazon.XRay.Recorder.Handlers.AwsSdk;

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
            context.Logger.LogLine("Hello, world!");

            var lambdaClient = new AmazonLambdaClient();

            InvokeResponse employee = await lambdaClient.InvokeAsync(new InvokeRequest
            {
                FunctionName = "fetch_employee_nanoservice"
            });
        }
    }
}
