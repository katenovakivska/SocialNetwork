using SocialNetwork.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.BLL.Validators
{
    public class MessageValidator
    {
        public static bool IsMessageValid(MessageDTO message)
        {
            return message.MessageText != null;
        }
    }
}
