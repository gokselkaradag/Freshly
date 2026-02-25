namespace Freshly.DomainLayer.Messages;

public class OcrJobMessage
{
    public Guid ProductId { get; set; }
    public string ImagePath { get; set; } = string.Empty;
}