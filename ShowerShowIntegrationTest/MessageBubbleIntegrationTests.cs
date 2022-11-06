using FluentAssertions;
using ShowerShow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ShowerShowIntegrationTest
{
    public class MessageBubbleIntegrationTests : ControllerBase
    {
        public MessageBubbleIntegrationTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
        }
        private Guid testMessageId = Guid.Parse("3bda7ff7-7c8d-47d2-92b6-7202f4c77c88");



        [Fact]
        public async Task GetRandomBubbleMessageReturnsARandomBubbleWithStatusCodeOk()
        {
            string requestUri = $"bubble/messages/retrieve";

            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<BubbleMessage>();
            assertVar.Should().NotBeNull();
            assertVar.Message.Should().NotBeNullOrEmpty();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetRandomMessagesListShouldReturn2OrMoreMessagesWithTheStatusCodeOK()
        {
            int limit = 2;
            string requestUri = $"bubble/messages/get/{limit}";

            var response = await client.GetAsync(requestUri);

            List<BubbleMessage> assertVar = await response.Content.ReadAsAsync<List<BubbleMessage>>();
            assertVar.Should().NotBeNull();
            assertVar.Count.Should().BeGreaterThanOrEqualTo(2);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }


        [Fact]
        public async Task GetRandomMessageByIdShouldReturnTheCorrectMessageWithStatusCodeOk()
        {
            string requestUri = $"bubble/messages/retrieve/{testMessageId}";

            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<BubbleMessage>();

            assertVar.Should().NotBeNull();
            assertVar.Id.Should().Be(testMessageId);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetRandomMessageByWrongIdShouldReturnBadRequest()
        {
            string requestUri = $"bubble/messages/retrieve/{Guid.NewGuid()}";

            var response = await client.GetAsync(requestUri);

            var assertVar = await response.Content.ReadAsAsync<BubbleMessage>();
            assertVar.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}