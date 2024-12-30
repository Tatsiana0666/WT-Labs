using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebTechLabs.DAL.Entities;

namespace WebTechLabs.Controllers
{
    public class ImageController : Controller
    {
        readonly UserManager<ApplicationUser> _userManager;
        readonly IWebHostEnvironment _env;

        public ImageController(UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }
        public async Task<FileResult> GetAvatar()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.AvatarImage != null)
                return File(user.AvatarImage, "image/...");
            else
            {
                var avatarPath = "/images/avatar.jpg";
                return File(_env.WebRootFileProvider.GetFileInfo(avatarPath).CreateReadStream(), "image/...");
            }
        }   
    }
}