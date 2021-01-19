using Amazon.Lambda.Core;
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
        public void Invoke(object input, ILambdaContext context)
        {
            context.Logger.LogLine("Hello, world!");
        }
    }
}
