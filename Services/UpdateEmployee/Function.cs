﻿using Amazon.Lambda.Core;

namespace UpdateEmployee
{
    public class Function
    {
        [Amazon.Lambda.Core.LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
        public void Invoke(object input, ILambdaContext content) {
            content.Logger.LogLine("Hello, world!");
        }
    }
}
