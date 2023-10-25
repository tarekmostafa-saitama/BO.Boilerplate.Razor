using Microsoft.AspNetCore.Http;
using Shared.Models.PaginateModels;

namespace Shared.Extensions;

public static class DataTableRequest
{
    // HttpRequestContext
    public static PaginateRequestModel MapToRequestModel(this HttpRequest request)
    {
        return new PaginateRequestModel
        {
            Draw = request.Form["draw"].FirstOrDefault<string?>(),
            Start = request.Form["start"].FirstOrDefault<string?>(),
            Length = request.Form["length"].FirstOrDefault<string?>(),
            SortColumn = request
                .Form["columns[" + request.Form["order[0][column]"].FirstOrDefault<string?>() + "][name]"]
                .FirstOrDefault<string?>(),
            SortColumnDirection = request.Form["order[0][dir]"].FirstOrDefault<string?>(),
            SearchValue = request.Form["search[value]"].FirstOrDefault<string?>()
        };
    }
}