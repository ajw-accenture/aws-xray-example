using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
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

        [LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
        public async Task Invoke(object input, ILambdaContext context)
        {
            context.Logger.LogLine("Hello, world!");

            var client = new AmazonSimpleNotificationServiceClient();

            var snsTopicArn = Environment.GetEnvironmentVariable("EMPLOYEE_MERGE_SAVE_SNS_ARN");
            await client.PublishAsync(new PublishRequest
            {
                TopicArn = snsTopicArn,
                Message = "{}"
            });
        }
    }
}
