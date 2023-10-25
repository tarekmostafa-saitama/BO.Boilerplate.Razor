namespace Shared.Models.PaginateModels;

public class PaginateModel<T>
{
    public PaginateRequestModel Request { get; set; }
    public T FilteredById { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}