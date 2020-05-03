

using SocialNetwork.BLL.DTOs;

namespace SocialNetwork.BLL.Validators
{
    public class RequestValidator
    {
        public static bool IsRequestValid(RequestDTO request)
        {
            return request.ReceiverName != null;
        }
    }
}
