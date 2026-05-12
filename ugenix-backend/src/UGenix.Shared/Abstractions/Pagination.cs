using System.Text;

namespace UGenix.Shared.Abstractions;

public record Cursor(string Value)
{
    public static Cursor From(string rawValue) => new(Convert.ToBase64String(Encoding.UTF8.GetBytes(rawValue)));
    public string Decode() => Encoding.UTF8.GetString(Convert.FromBase64String(Value));
}

public record CursorPaginationRequest(
    Cursor? After = null,
    Cursor? Before = null,
    int PageSize = 20,
    string? OrderBy = null,
    bool IsAscending = false);

public record PagedListMetadata(
    Cursor? StartCursor,
    Cursor? EndCursor,
    bool HasNextPage,
    bool HasPreviousPage,
    int TotalCount);

public class PagedList<T>
{
    public IReadOnlyCollection<T> Items { get; }
    public PagedListMetadata Metadata { get; }

    public PagedList(IReadOnlyCollection<T> items, PagedListMetadata metadata)
    {
        Items = items;
        Metadata = metadata;
    }
}

