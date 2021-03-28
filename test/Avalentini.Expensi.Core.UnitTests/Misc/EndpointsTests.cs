using Avalentini.Expensi.Core.Misc;
using Xunit;

namespace Avalentini.Expensi.Core.UnitTests.Misc
{
    public class EndpointsTests
    {
        [Fact]
        public void UrlCombineTest()
        {
            Assert.Equal("test1/test2", Endpoints.UrlCombine("test1", "test2"));
            Assert.Equal("test1/test2", Endpoints.UrlCombine("test1/", "test2"));
            Assert.Equal("test1/test2", Endpoints.UrlCombine("test1", "/test2"));
            Assert.Equal("test1/test2", Endpoints.UrlCombine("test1/", "/test2"));
            Assert.Equal("/test1/test2/", Endpoints.UrlCombine("/test1/", "/test2/"));
            Assert.Equal("/test2/", Endpoints.UrlCombine("", "/test2/"));
            Assert.Equal("/test1/", Endpoints.UrlCombine("/test1/", ""));
        }
    }
}
