using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DAL.Entities;
using System.Web;

namespace SocialNetwork.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        UserManager<ApplicationUser> _userManager;
        public PhotoController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        //public async Task<IActionResult> OnPostUploadAsync()
        //{
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        await FileUpload.FormFile.CopyToAsync(memoryStream);

        //        // Upload the file if less than 2 MB
        //        if (memoryStream.Length < 2097152)
        //        {
        //            var file = new AppFile()
        //            {
        //                Content = memoryStream.ToArray()
        //            };

        //            _dbContext.File.Add(file);

        //            await _dbContext.SaveChangesAsync();
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("File", "The file is too large.");
        //        }
        //    }

        //    return Page();
        //}
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddAvatar()
        {

            var avatar = Request.Form.Files[0];
            // Person person = new Person { Name = pvm.Name };
            var user = _userManager.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            if (avatar == null)
            {
                return null;
            }

            byte[] imageData = null;
            // считываем переданный файл в массив байтов
            using (var binaryReader = new BinaryReader(avatar.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int)avatar.Length);
            }
            // установка массива байтов

            user.Avatar = imageData;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return NoContent();
        }


        //[Authorize]
        //[HttpPost]
        //public async Task<IActionResult> AddAvatar(IFormFile formFile)
        //{
        //    if (formFile.Length > 0)
        //    {
        //        var user = _userManager.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
        //        using (var stream = new MemoryStream())
        //        {
        //            await formFile.CopyToAsync(stream);
        //            user.Avatar = stream.ToArray();
        //            var result = await _userManager.UpdateAsync(user);

        //            if (!result.Succeeded)
        //            {
        //                return BadRequest();
        //            }

        //        }

        //    }
        //    return NoContent();
        //}
    }
}