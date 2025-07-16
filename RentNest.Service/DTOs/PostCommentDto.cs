using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.DTOs
{
    public class PostCommentDto
    {
        public int CommentId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }

        public string AccountName { get; set; }
        public string AccountAvatarUrl { get; set; }

        public int? ParentCommentId { get; set; }
        public List<PostCommentDto> Replies { get; set; } = new();
    }
}
