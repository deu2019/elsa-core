using Elsa.Bookmarks;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Activities.AzureServiceBus.Bookmarks
{
    public class TopicMessageReceivedBookmark : IBookmark
    {
        public TopicMessageReceivedBookmark()
        {
        }

        public TopicMessageReceivedBookmark(string topicName, string subscriptionName, string? correlationId = default)
        {
            TopicName = topicName;
            SubscriptionName = subscriptionName;
            CorrelationId = correlationId;
        }

        public string TopicName { get;  set; } = default!;
        public string SubscriptionName { get; set; } = default!;
        public string? CorrelationId { get; set; }
    }

    public class TopicMessageReceivedBookmarkProvider : BookmarkProvider<TopicMessageReceivedBookmark, AzureServiceBusTopicMessageReceived>
    {
        public override async ValueTask<IEnumerable<BookmarkResult>> GetBookmarksAsync(BookmarkProviderContext<AzureServiceBusTopicMessageReceived> context, CancellationToken cancellationToken) =>
            new[]
            {
                Result(new TopicMessageReceivedBookmark
                {
                    TopicName = (await context.ReadActivityPropertyAsync(x => x.TopicName, cancellationToken))!,
                    SubscriptionName = (await context.ReadActivityPropertyAsync(x => x.SubscriptionName, cancellationToken))!,
                    CorrelationId = context.ActivityExecutionContext.WorkflowExecutionContext.CorrelationId
                })
            };
    }
}