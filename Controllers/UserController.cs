using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using UserManagementApp.Models;

[Authorize]
public class UserManagementController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager; // Add this line

    public UserManagementController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) // Update constructor
    {
        _userManager = userManager;
        _signInManager = signInManager; // Initialize _signInManager
    }

    public async Task<IActionResult> Index()
    {
        // Retrieve all users from AspNetUsers
        var users = await _userManager.Users.ToListAsync();
        return View(users);
    }

    [HttpPost]
    public async Task<IActionResult> ManageUsers(List<string> userIds, string action)
    {
        var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
        var currentUserId = _userManager.GetUserId(User);
        bool isCurrentUserAffected = currentUserId != null && userIds.Contains(currentUserId);

        switch (action)
        {
            case "Block":
                foreach (var user in users)
                {
                    user.LockoutEnabled = true;
                    user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
                    await _userManager.UpdateAsync(user);
                }
                break;

            case "Unblock":
                foreach (var user in users)
                {
                    user.LockoutEnd = null;
                    await _userManager.UpdateAsync(user);
                }
                break;

            case "Delete":
                foreach (var user in users)
                {
                    await _userManager.DeleteAsync(user);
                }
                break;
        }

        if (isCurrentUserAffected && (action == "Block" || action == "Delete"))
        {
            await _signInManager.SignOutAsync();
            return Json(new { success = true, logout = true });
        }

        return Json(new { success = true });
    }
}