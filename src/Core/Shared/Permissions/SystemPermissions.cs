using System.Collections.ObjectModel;

namespace Shared.Permissions;

public static class Actions
{
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string Export = nameof(Export);
}

public static class Resources
{
    public const string Users = nameof(Users);
    public const string Roles = nameof(Roles);
    public const string Transactions = nameof(Transactions);
}

public static class Permissions
{
    private static readonly Permission[] _all =
    {
        new("View Users", Actions.View, Resources.Users, true),
        new("Create Users", Actions.Create, Resources.Users, true),
        new("Update Users", Actions.Update, Resources.Users, true),
        new("Delete Users", Actions.Delete, Resources.Users, true),
        new("Export Users", Actions.Export, Resources.Users, true),

        new("View Roles", Actions.View, Resources.Roles, true),
        new("Create Roles", Actions.Create, Resources.Roles, true),
        new("Update Roles", Actions.Update, Resources.Roles, true),
        new("Delete Roles", Actions.Delete, Resources.Roles, true),
        new("Export Roles", Actions.Export, Resources.Roles, true),

        new("View Transactions", Actions.View, Resources.Transactions, true),
        new("Export Transactions", Actions.Export, Resources.Transactions, true)
    };

    public static IReadOnlyDictionary<string, List<Permission>> CategorizedPermissions =
        _all.GroupBy(x => x.Resource).ToDictionary(x => x.Key, y => y.Select(z => z).ToList());

    public static IReadOnlyList<Permission> All { get; } = new ReadOnlyCollection<Permission>(_all);

    public static IReadOnlyList<Permission> Admin { get; } =
        new ReadOnlyCollection<Permission>(_all.Where(p => p.IsAdmin).ToArray());
}

public record Permission(string Description, string Action, string Resource, bool IsAdmin = false)
{
    public string Name => NameFor(Action, Resource);

    public static string NameFor(string action, string resource)
    {
        return $"Permissions.{resource}.{action}";
    }
}