namespace Freshly.API.Dtos;

    public record AuthResponse(string Token, string Email, Guid UserId);
    public record ProductResponse(Guid Id, string Name, string ImagePath, DateTime? ExpiraitonDate, string Status, DateTime CreatedAt);
    public record MessageResponse(string Message);
