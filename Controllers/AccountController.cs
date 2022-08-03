using System.Web;
using BlazorServerTest.Data.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BlazorServerTest.Controllers; 

public class AccountController : ControllerBase {
    private readonly ILogger<AccountController> _logger;
    private readonly IAppUserManager _userManager;
    private readonly IDataProtectionProvider _dataProtectionProvider;

    public AccountController(ILogger<AccountController> logger, IAppUserManager userManager,
        IDataProtectionProvider dataProtectionProvider) {
        _logger = logger;
        _userManager = userManager;
        _dataProtectionProvider = dataProtectionProvider;
    }
    
    [AllowAnonymous]
    [HttpGet("/auth/login")]
    public async Task<IActionResult> Login(string token, string redirect) {
        
        if ( string.IsNullOrEmpty(token) ) return Redirect("/");
        if ( string.IsNullOrEmpty(redirect) ) redirect = "/";
        
        var dataProtector = _dataProtectionProvider.CreateProtector("Login");
        var data = dataProtector.Unprotect(Base64UrlEncoder.Decode(token));
        var parts = data.Split('|');
        
        var identityUser = await _userManager.GetUserByIdAsync(parts[0]);
        var isTokenValid = await _userManager.VerifyUserTokenForLoginAsync(identityUser, TokenOptions.DefaultProvider, parts[1]);

        if ( !isTokenValid ) {
            _logger.LogInformation("Failed to authorize user");
            return Unauthorized();
        }

        await _userManager.ResetAccessFailedCountAsync(identityUser);
        await _userManager.SignInAsync(identityUser, true);

        _logger.LogInformation("Successfully authorized user. Redirecting...");
        
        return Redirect(redirect);
    }

    [HttpGet("/auth/logout")]
    public async Task<IActionResult> Logout(string redirect) {
        if ( string.IsNullOrEmpty(redirect) ) redirect = "/";

        await _userManager.SignOutAsync();
        
        return Redirect(redirect);
    }
    
    [AllowAnonymous]
    [HttpGet("/auth/verify")]
    public async Task<IActionResult> Verify(string? email, string token) {
        if ( string.IsNullOrEmpty(token) ) return BadRequest();

        var user = await _userManager.GetUserByEmailAsync(email);
        if ( user == null ) return BadRequest();
        
        var result = await _userManager.ConfirmEmailAsync(user, token);
        if ( !result.Succeeded ) return StatusCode(500);
        
        _logger.LogInformation("Successfully verified email: {Address}", email);
            
        return Redirect("/");
    }
}