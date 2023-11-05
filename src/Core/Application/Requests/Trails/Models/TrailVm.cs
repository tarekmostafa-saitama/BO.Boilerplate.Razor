using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Trails.Models;

public class TrailVm
{
    public int Id { get; set; }

    [Display(Name = "transactions_userId")]
    public string UserId { get; set; }

    [Display(Name = "transactions_type")]
    public string Type { get; set; }

    [Display(Name = "transactions_tableName")]
    public string TableName { get; set; }

    [Display(Name = "transactions_dateTime")]
    public DateTime DateTime { get; set; }

    [Display(Name = "transactions_oldValues")]
    public string OldValues { get; set; }

    [Display(Name = "transactions_newValues")]
    public string NewValues { get; set; }

    [Display(Name = "transactions_affectedColumns")]
    public string AffectedColumns { get; set; }

    [Display(Name = "transactions_primaryKey")]
    public string PrimaryKey { get; set; }
}