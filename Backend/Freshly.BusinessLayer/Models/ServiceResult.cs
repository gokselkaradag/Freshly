namespace Freshly.BusinessLayer.Models;

public record AuthResult(bool Success, string Token, string? Email, Guid? UserId, string? Error);

public record ProductUploadResult(Guid ProductId, string Status, string Message);

public record ProductResult(
    Guid Id,
    string Name,
    string ImagePath,
    DateTime? ExpirationDate,
    string Status,
    DateTime CreatedAt
    );