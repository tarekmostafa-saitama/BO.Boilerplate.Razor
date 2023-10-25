namespace Shared.Models.PaginateModels;

public class PaginateRequestModel
{
    public string Draw { get; set; }
    public string Start { get; set; }
    public string Length { get; set; }
    public string SortColumn { get; set; }
    public string SortColumnDirection { get; set; }
    public string SearchValue { get; set; }
}