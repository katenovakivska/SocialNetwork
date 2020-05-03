using SocialNetwork.BLL.DTOs;

namespace SocialNetwork.BLL.Validators
{
    public class CommentValidator
    {
        public static bool IsCommentValid(CommentDTO comment)
        {
            return comment.CommentText != null;
        }
    }
}
