using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.XRay.Recorder.Handlers.AwsSdk;

namespace MergeEmployee
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

            var client = new AmazonSimpleNotificationServiceClient();

            var snsTopicArn = context.ClientContext.Environment["EMPLOYEE_MERGE_SAVE_SNS_ARN"];
            await client.PublishAsync(new PublishRequest {
                TopicArn = snsTopicArn,
                Message = "{}"
            });
        }
    }
}
