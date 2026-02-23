using System.ComponentModel.DataAnnotations;

namespace Hotel.Web.ViewModels;

public sealed class EventInquiryInputModel
{
    [Required(ErrorMessage = "Validation.Required")]
    [StringLength(120, ErrorMessage = "Validation.StringLength")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Validation.Required")]
    [EmailAddress(ErrorMessage = "Validation.Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Validation.Required")]
    [Phone(ErrorMessage = "Validation.Phone")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Validation.Required")]
    [DataType(DataType.Date)]
    public DateTime? Date { get; set; }

    [Required(ErrorMessage = "Validation.Required")]
    [StringLength(1500, ErrorMessage = "Validation.StringLength")]
    public string Message { get; set; } = string.Empty;
}

public sealed class ContactInputModel
{
    [Required(ErrorMessage = "Validation.Required")]
    [StringLength(120, ErrorMessage = "Validation.StringLength")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Validation.Required")]
    [EmailAddress(ErrorMessage = "Validation.Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Validation.Required")]
    [StringLength(200, ErrorMessage = "Validation.StringLength")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Validation.Required")]
    [StringLength(1500, ErrorMessage = "Validation.StringLength")]
    public string Message { get; set; } = string.Empty;
}

public sealed class EventsPageViewModel
{
    public required PageContextViewModel Page { get; init; }
    public required EventInquiryInputModel Form { get; init; }
    public bool IsSubmitted { get; init; }
}

public sealed class ContactPageViewModel
{
    public required PageContextViewModel Page { get; init; }
    public required ContactInputModel Form { get; init; }
    public bool IsSubmitted { get; init; }
}
