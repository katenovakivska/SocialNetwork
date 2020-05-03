using SocialNetwork.BLL.DTOs;

namespace SocialNetwork.BLL.Validators
{
    public class LikeValidator
    {
        public static bool IsLikeValid(LikeDTO like)
        {
            return like.UserName != null;
        }
    }
}
