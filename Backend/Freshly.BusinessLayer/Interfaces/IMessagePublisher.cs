using Freshly.DomainLayer.Messages;

namespace Freshly.BusinessLayer.Interfaces;

public interface IMessagePublisher
{
    Task PublishOcrJobAsync(OcrJobMessage message);
}