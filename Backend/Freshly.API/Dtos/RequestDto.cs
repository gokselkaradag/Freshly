namespace Freshly.API.Dtos;

    public record RegisterRequest(string Email, string Password);
    public record LoginRequest(string Email, string Password);
    public record UploadProductRequest(string Name);
    public record SetExpirationRequest(DateTime ExpiraitonDate);
    public record DeviceTokenRequest(string Token, string Platform);