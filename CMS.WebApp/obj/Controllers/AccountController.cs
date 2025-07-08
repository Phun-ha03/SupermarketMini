using CMS.Models.Authen.Functions;
using CMS.Models.Authen.Users;
using CMS.Services.Authen.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace CMS.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IFunctionService _functionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountController(
            IConfiguration configuration,
            IUserService userService,
            IFunctionService functionService,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _configuration = configuration;
            _userService = userService;
            _functionService = functionService;
            _httpContextAccessor = httpContextAccessor;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login([FromQuery] string returnUrl)
        {
            try
            {
                await _userService.SignOut();

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                _httpContextAccessor.HttpContext?.Session.Remove(_configuration["SessionKeys:Menu"]);
                _httpContextAccessor.HttpContext?.Session.Remove(_configuration["SessionKeys:UserData"]);

                LoginRequest request = new LoginRequest();
                request.ReturnUrl = string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl;

                return View(request);
            }
            catch (Exception ex)
            {
                Utilities.Helpers.LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _userService.AuthenticateV2(request);

                if (result.IsSuccessed && result.ResultObj != null && result.ResultObj.Id > 0)
                {
                    var userViewModel = result.ResultObj;

                    // create claims
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, userViewModel.Id.ToString()),
                        new Claim(ClaimTypes.Name, userViewModel.UserName),
                    };

                    // create identity
                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // create principal
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    // sign-in
                    await HttpContext.SignInAsync(
                            scheme: CookieAuthenticationDefaults.AuthenticationScheme,
                            principal: principal,
                            properties: new AuthenticationProperties
                            {
                                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(720),
                                IsPersistent = request.RememberMe,
                                AllowRefresh = true
                            });
                    try
                    {
                        _httpContextAccessor.HttpContext?.Session.SetString(_configuration["SessionKeys:UserData"], JsonConvert.SerializeObject(userViewModel));

                        /*if (request.RememberMe) {
                            HttpContext.Response.Cookies.Append("UserData", JsonConvert.SerializeObject(userViewModel), new Microsoft.AspNetCore.Http.CookieOptions
                            {
                                Expires = DateTime.Now.AddDays(365)
                            });
                        }*/
                    }
                    catch (Exception ex) { }
                    try
                    {
                        var menus = _functionService.GetAllActiveUserMenus(new GetUserMenuRequest()
                        {
                            UserId = userViewModel.Id
                        }).Result.ResultObj;
                        if (menus != null && menus.Count > 0)
                        {
                            try
                            {
                                _httpContextAccessor.HttpContext?.Session.SetString(_configuration["SessionKeys:Menu"], JsonConvert.SerializeObject(menus));

                                /*if (request.RememberMe)
                                {
                                    HttpContext.Response.Cookies.Append("Menu", JsonConvert.SerializeObject(menus), new Microsoft.AspNetCore.Http.CookieOptions
                                    {
                                        Expires = DateTime.Now.AddDays(365)
                                    });
                                }*/
                            }
                            catch (Exception ex) { }
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    if (!string.IsNullOrEmpty(request.ReturnUrl) && !request.ReturnUrl.Equals("/"))
                    {
                        return Redirect(request.ReturnUrl);
                    }
                    return RedirectToAction("Index", "User");
                }
                if (!string.IsNullOrEmpty(result.Message))
                {
                    if (result.Message.ToLower().Contains("mật khẩu")
                        || result.Message.ToLower().Contains("password"))
                    {
                        ModelState.AddModelError("Password", result.Message);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", result.Message);
                    }
                }

                return View(request);
            }
            catch (Exception ex)
            {
                Utilities.Helpers.LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        /*[HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _userService.Authenticate(request);

                if (result.IsSuccessed && !string.IsNullOrEmpty(result.ResultObj))
                {
                    var token = result.ResultObj;
                    var userPrincipal = this.ValidateToken(token);
                    var authProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                        IsPersistent = request.RememberMe
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties
                        );

                    //string roles = userPrincipal.FindFirstValue(ClaimTypes.UserData);
                    //HttpContext.Session.SetString(_configuration["SessionKeys:Token"], token);
                    //HttpContext.Session.SetString(_configuration["SessionKeys:Roles"], roles);

                    try
                    {
                        var menus = (_functionService.GetUserMenus(new GetUserMenuRequest()
                        {
                            UserId = Convert.ToInt32(userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier))
                        })).Result.ResultObj;
                        if (menus != null && menus.Count > 0)
                        {
                            try
                            {
                                HttpContext.Session.SetString(_configuration["SessionKeys:Menu"], JsonConvert.SerializeObject(menus));
                            }
                            catch (Exception ex){}
                        }
                    }
                    catch (Exception ex) { 
                    }

                    if (!string.IsNullOrEmpty(request.ReturnUrl))
                    {
                        return Redirect(request.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                if (!string.IsNullOrEmpty(result.Message))
                {
                    if (result.Message.ToLower().Contains("mật khẩu")
                        || result.Message.ToLower().Contains("password"))
                    {
                        ModelState.AddModelError("Password", result.Message);
                    }
                    else
                    {
                        ModelState.AddModelError("UserName", result.Message);
                    }
                }

                return View(request);
            }
            catch (Exception ex)
            {
                ICSoft.Jobman.Utilities.Helpers.LogHelper.writeLog(ex.ToString(), ((new System.Diagnostics.StackTrace()).GetFrames()[0]).GetMethod().Name);
                throw;
            }
        }*/

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            try
            {
                IdentityModelEventSource.ShowPII = true;

                SecurityToken validatedToken;
                TokenValidationParameters validationParameters = new TokenValidationParameters();

                validationParameters.ValidateLifetime = true;
                validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
                validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
                validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

                ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

                return principal;
            }
            catch (Exception ex)
            {
                Utilities.Helpers.LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _userService.SignOut();
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.Session.Remove(_configuration["SessionKeys:Token"]);
                HttpContext.Session.Remove(_configuration["SessionKeys:Menu"]);
                HttpContext.Session.Remove(_configuration["SessionKeys:curr_menu"]);

                Response.Cookies.Delete("UserData");
                Response.Cookies.Delete("Menu");

                _httpContextAccessor.HttpContext?.Session.Clear();

                //HttpContext.Session.Clear();
                return RedirectToAction("login", "account");
            }
            catch (Exception ex)
            {
                Utilities.Helpers.LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassWord()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassWord(ForgotPassWordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _userService.ForgotPassword(request);
                if (result.IsSuccessed)
                {
                    // Cấu hình thông tin tài khoản Gmail
                    string email = AppSettings.SendMessageMail;
                    string password = AppSettings.SendMessageMailPass;

                    string link = $"{AppSettings.MainDomain}account/resetpassword?{result.ResultObj}";

                    // Tạo đối tượng MailMessage
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(email);
                    mail.To.Add(request.Email);
                    mail.Subject = "Đặt lại mật khẩu";
                    mail.Body = $"Xin chào, <br/><br/>Bạn vừa yêu cầu đặt lại mật khẩu trên {AppSettings.MainDomain}.<br/><br/>Để đặt lại mật khẩu, bạn vui lòng click vào liên kết bên dưới và làm theo hướng dẫn.<br/><a href=\"{link}\">Click vào đây để đặt lại mật khẩu.</a><br/><br/>Bỏ qua email này nếu đây không đúng là bạn!<br/><br/><b>Thanks.</b>";
                    mail.IsBodyHtml = true;

                    // Thiết lập thông tin SMTP Server của Gmail
                    SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential(email, password);
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;

                    try
                    {
                        // Gửi email
                        smtpClient.Send(mail);
                        ViewBag.Message = "Link đặt lại mật khẩu đã được gửi về địa chỉ email " + request.Email + ". Vui lòng check mail và làm theo hướng dẫn!";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Error = "Quá trình gửi mail xảy ra lỗi. Xin vui lòng thử lại!";
                    }
                }
                else
                {
                    ViewBag.Error = "Lỗi yêu cầu đặt lại mật khẩu. Xin vui lòng thử lại!";
                }

                return View(request);
            }
            catch (Exception ex)
            {
                Utilities.Helpers.LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(int u, string token)
        {
            ResetPasswordRequest request = new ResetPasswordRequest()
            {
                Id = u,
                Token = token
            };
            return View(request);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _userService.ResetPassword(request);
                if (result.IsSuccessed)
                {
                    ViewBag.Message = "Đặt lại mật khẩu thành công!";
                }
                else
                {
                    ViewBag.Error = string.IsNullOrEmpty(result.Message) ? "Lỗi yêu cầu đặt lại mật khẩu. Xin vui lòng thử lại!" : result.Message;
                }

                return View(request);
            }
            catch (Exception ex)
            {
                Utilities.Helpers.LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
    }
}
