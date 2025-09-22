namespace Application.Abstractions;

public sealed record CursorResponse<T>(long Cursor, T Data);
