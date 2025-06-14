using System;
using System.Collections.Generic;

namespace RentNest.Core.Domains;

public partial class PostComment
{
    public int CommentId { get; set; }

    public int PostId { get; set; }

    public int AccountId { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? ParentCommentId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<PostComment> InverseParentComment { get; set; } = new List<PostComment>();

    public virtual PostComment? ParentComment { get; set; }

    public virtual Post Post { get; set; } = null!;
}
