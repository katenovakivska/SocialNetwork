using SocialNetwork.BLL.DTOs;

namespace SocialNetwork.BLL.Validators
{
    public class PublicationValidator
    {
        public static bool IsPublicationValid(PublicationDTO publication)
        {
            return publication.PublicationText != null;
        }
    }
}
