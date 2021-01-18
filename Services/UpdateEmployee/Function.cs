using Amazon.Lambda.Core;

namespace UpdateEmployee
{
    public class Function
    {
        public void Invoke(object input, ILambdaContext content) {
            content.Logger.LogLine("Hello, world!");
        }
    }
}
