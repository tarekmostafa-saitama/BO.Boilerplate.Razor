using Application.Common.Models.UserModels;

namespace Application.Requests.UsersManagement.Models;

public class PaginateUsersVm
{
    public PaginateUsersVm(int pageNumber = 1, int pageSize = 20)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Users = new List<UserVm>();
    }

    public List<UserVm> Users { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}