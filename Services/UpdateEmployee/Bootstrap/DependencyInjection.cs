using System;
using Amazon.XRay.Recorder.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using UpdateEmployee.Services;

namespace UpdateEmployee.Bootstrap
{
    public class DependencyInjection
    {
        public static IServiceProvider Initialize()
        {
            var recorder = AWSXRayRecorder.Instance;
            recorder.BeginSubsegment("SEG Hydrate Service Provider");

            var provider = BuildProvider();

            recorder.EndSubsegment();
            return provider;
        }

        private static IServiceProvider BuildProvider()
        {
            var servicesCollection = new ServiceCollection();

            servicesCollection.AddSingleton(AWSXRayRecorder.Instance);

            servicesCollection.AddLogging((loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                loggingBuilder.AddNLog();
            });

            servicesCollection.AddSingleton<IFetchEmployeeService, FetchEmployeeService>();
            servicesCollection.AddSingleton<IMergeEmployeeService, MergeEmployeeService>();

            var provider = servicesCollection.BuildServiceProvider();

            return provider;
        }
    }
}
