namespace Freshly.DomainLayer.Enums;

public enum ProductStatus
{
    PendingOCR = 0,
    OCRCompleted = 1,
    NeedsManualInput = 2,
    Failed = 3
}