using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.UserModels;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Extensions;
using Shared.Models.PaginateModels;

namespace Infrastructure.Identity.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<PaginateResponseModel<UserVm>> GetUsersAsync(PaginateModel<string> model)
    {
        var pageSize = model.Request.Length != null ? Convert.ToInt32(model.Request.Length) : 0;
        var skip = model.Request.Start != null ? Convert.ToInt32(model.Request.Start) : 0;
        var recordsTotal = 0;

        var usersQuery = _userManager.Users;
        if (model.From != null) usersQuery = usersQuery.Where(x => x.CreatedOn >= model.From);
        if (model.To != null) usersQuery = usersQuery.Where(x => x.CreatedOn <= model.To);

        if (!string.IsNullOrWhiteSpace(model.Request.SearchValue))
            usersQuery = usersQuery.Where(x => x.FullName.Contains(model.Request.SearchValue)
                                               || x.Email.Contains(model.Request.SearchValue));

        if (!string.IsNullOrWhiteSpace(model.Request.SortColumn))
            usersQuery = model.Request.SortColumnDirection == "asc"
                ? usersQuery.OrderBy(model.Request.SortColumn)
                : usersQuery.OrderByDescending(model.Request.SortColumn);
        else
            usersQuery = usersQuery.OrderByDescending(x => x.CreatedOn);

        recordsTotal = await usersQuery.CountAsync();
        var users = await usersQuery.Skip(skip).Take(pageSize).ToListAsync();
        return new PaginateResponseModel<UserVm>
        {
            Draw = model.Request.Draw,
            RecordsFiltered = recordsTotal,
            RecordsTotal = recordsTotal,
            Data = users.Adapt<List<UserVm>>()
        };
    }

    public async Task<PaginateResponseModel<UserVm>> GetUsersInRoleAsync(PaginateModel<string> model)
    {
        throw new NotImplementedException();
    }

    public async Task<UserVm> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null) return user.Adapt<UserVm>();
        throw new NotFoundException("user not found with id= " + userId);
    }

    public async Task<UserVm> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null) return user.Adapt<UserVm>();
        throw new NotFoundException("user not found with email= " + email);
    }


    public async Task<IResponse<string>> CreateUserAsync(CreateUserVm userVm)
    {
        var user = new ApplicationUser()
        {
            FullName = userVm.FullName,
            Email = userVm.Email,
            UserName = userVm.Email,
        };
        var result = await _userManager.CreateAsync(user, userVm.Password);
        if (result.Succeeded)
            return Response.Success(user.Id);
        return Response.Fail<string>(result.Errors.Select(x => x.Description).ToList());
    }

    public async Task<IResponse<string>> UpdateUserAsync( UpdateUserVm userVm)
    {
        var user = await _userManager.FindByIdAsync(userVm.Id) ??
                   throw new NotFoundException("user not found with id= " + userVm.Id);
        user.FullName = userVm.FullName;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
            return Response.Success(user.Id);
        return Response.Fail<string>(result.Errors.Select(x => x.Description).ToList());
    }

    public async Task DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) 
         throw new NotFoundException("user not found with id= " + userId);
        await _userManager.DeleteAsync(user);
    }

    public async Task ReplaceRolesToUserAsync(string userId, List<string> roles)
    {
        var user = await _userManager.FindByIdAsync(userId) ??
                   throw new NotFoundException("user not found with id= " + userId);
        var currentRoles = await _userManager.GetRolesAsync(user);

        await _userManager.RemoveFromRolesAsync(user, currentRoles);

        await _userManager.AddToRolesAsync(user, roles);
    }
}