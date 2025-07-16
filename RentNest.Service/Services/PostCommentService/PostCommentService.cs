using RentNest.Core.Domains;
using RentNest.Infrastructure.Repositories.PostCommentRepo;
using RentNest.Service.DTOs;

namespace RentNest.Service.Services.PostCommentService
{
    public class PostCommentService : IPostCommentService
    {
        private readonly IPostCommentRepository _postCommentRepository;

        public PostCommentService(IPostCommentRepository postCommentRepository)
        {
            _postCommentRepository = postCommentRepository;
        }

        public async Task AddCommentAsync(PostComment comment)
        {
            await _postCommentRepository.AddAsync(comment);
        }

        public async Task<List<PostCommentDto>> GetCommentsByPostIdAsync(int postId)
        {
            var comments = await _postCommentRepository.GetCommentsByPostIdAsync(postId);

            return comments.Select(c => new PostCommentDto
            {
                CommentId = c.CommentId,
                Content = c.Comment,
                CreatedAt = c.CreatedAt,
                AccountName = c.Account?.UserProfile?.FirstName + " " + c.Account?.UserProfile?.LastName,
                AccountAvatarUrl = c.Account?.UserProfile?.AvatarUrl ?? "/images/default-avatar.jpg",
                ParentCommentId = c.ParentCommentId,
                Replies = c.InverseParentComment.Select(r => new PostCommentDto
                {
                    CommentId = r.CommentId,
                    Content = r.Comment,
                    CreatedAt = r.CreatedAt,
                    AccountName = r.Account?.UserProfile?.FirstName + " " + r.Account?.UserProfile?.LastName,
                    AccountAvatarUrl = r.Account?.UserProfile?.AvatarUrl ?? "/images/default-avatar.jpg",
                    ParentCommentId = r.ParentCommentId
                }).ToList()
            }).ToList();
        }

    }
}
