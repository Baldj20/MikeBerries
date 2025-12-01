using Polly;
using Polly.Registry;

namespace ProductService.IntegrationTests;

public class FakeResiliencePipelineProvider : ResiliencePipelineProvider<string>
{
    public override ResiliencePipeline GetPipeline(string key)
    {
        return ResiliencePipeline.Empty;
    }

    public override ResiliencePipeline<TResult> GetPipeline<TResult>(string key)
    {
        return ResiliencePipeline<TResult>.Empty;
    }

    public override bool TryGetPipeline(string key, out ResiliencePipeline pipeline)
    {
        pipeline = ResiliencePipeline.Empty;
        return true;
    }

    public override bool TryGetPipeline<TResult>(string key, out ResiliencePipeline<TResult> pipeline)
    {
        pipeline = ResiliencePipeline<TResult>.Empty;
        return true;
    }
}
