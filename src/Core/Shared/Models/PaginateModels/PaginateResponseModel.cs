namespace Shared.Models.PaginateModels;

public class PaginateResponseModel<T> where T : class
{
    public string Draw { get; set; }
    public int RecordsFiltered { get; set; }
    public int RecordsTotal { get; set; }
    public List<T> Data { get; set; }
}