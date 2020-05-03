using SocialNetwork.BLL.DTOs;

namespace SocialNetwork.BLL.Validators
{
    public class FriendshipValidator
    {
        public static bool IsFriendshipValid(FriendshipDTO friendship)
        {
            return friendship.FriendName != null;
        }
    }
}
