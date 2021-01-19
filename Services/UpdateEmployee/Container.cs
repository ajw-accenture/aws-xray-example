using System;
using Amazon.XRay.Recorder.Core;
using Microsoft.Extensions.DependencyInjection;

namespace UpdateEmployee
{
    public class Container
    {
        public static IServiceProvider Initialize()
        {
            var recorder = AWSXRayRecorder.Instance;
            recorder.BeginSubsegment("Initialize: stand up dependency injection container");

            var provider = BuildProvider();

            recorder.EndSubsegment();
            return provider;
        }

        private static IServiceProvider BuildProvider()
        {
            var servicesCollection = new ServiceCollection();

            servicesCollection.AddSingleton(AWSXRayRecorder.Instance);

            servicesCollection.AddLogging();

            servicesCollection.AddSingleton<IFetchEmployeeService, FetchEmployeeService>();

            var provider = servicesCollection.BuildServiceProvider();

            return provider;
        }
    }
}
