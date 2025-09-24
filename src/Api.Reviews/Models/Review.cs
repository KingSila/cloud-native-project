using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Reviews.Models;

public class Review
{
    public int Id { get; set; }                 // PK
    public int BookId { get; set; }             // foreign key reference to Catalog service’s Book
    public string UserId { get; set; } = "";    // from auth (B2C subject) or placeholder for now
    public int Stars { get; set; }              // 1..5
    public string Title { get; set; } = "";
    public string Body { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ReviewStatus Status { get; set; } = ReviewStatus.Published; // simple moderation
}

public enum ReviewStatus { Pending = 0, Published = 1, Rejected = 2 }
