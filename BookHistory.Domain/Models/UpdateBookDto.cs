namespace BookHistory.Domain.Models;

using System.ComponentModel.DataAnnotations;

public class UpdateBookDto
{
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Publish date is required")]
    public DateOnly PublishDate { get; set; }

    [MinLength(1, ErrorMessage = "At least one author is required")]
    public List<string> Authors { get; set; } = new();
}