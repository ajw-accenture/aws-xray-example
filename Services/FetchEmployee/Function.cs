using System;
using Amazon.Lambda.Core;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using Shared.Models;

namespace FetchEmployee
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
        public Employee Invoke(object input, ILambdaContext context)
        {
            return new Employee {
                PersonnelId = Guid.NewGuid().ToString(),
                Name = "Dwight Shrute",
                Department = "Nobody knows, not even Dwight"
            };
        }
    }
}
