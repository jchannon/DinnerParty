using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Extensions;
using Nancy.Validation;
using Nancy.ModelBinding;
using DinnerParty.Models;

namespace DinnerParty.Modules
{
    public class AccountModule : BaseModule
    {
        public AccountModule()
            : base("/account")
        {
            Get["/logon"] = parameters =>
            {
                base.Page.Title = "Login";

                var loginModel = new LoginModel();
                base.Model.LoginModel = loginModel;

                return View["LogOn", base.Model];
            };

            Post["/logon"] = parameters =>
                {
                    var model = this.Bind<LoginModel>();
                    var result = this.Validate(model);

                    var userGuid = UserMapper.ValidateUser(model.UserName, model.Password);

                    if (userGuid == null || !result.IsValid)
                    {
                        base.Page.Title = "Login";
                        base.Model.Page.Errors = result.Errors;


                        base.Model.LoginModel = model;

                        return View["LogOn", base.Model];
                    }

                    DateTime? expiry = null;
                    if (model.RememberMe)
                    {
                        expiry = DateTime.Now.AddDays(7);
                    }

                    return this.LoginAndRedirect(userGuid.Value, expiry);
                };

            Get["/logoff"] = parameters =>
                {
                    return this.LogoutAndRedirect("/");
                };

            Get["/register"] = parameters =>
            {
                base.Page.Title = "Register";

                var registerModel = new RegisterModel();
                base.Model.RegisterModel = registerModel;


                return View["Register", base.Model];
            };

            Post["/register"] = parameters =>
                {
                    var model = this.Bind<RegisterModel>();
                    var result = this.Validate(model);

                    if (!result.IsValid)
                    {
                        base.Page.Title = "Register";

                        base.Model.RegisterModel = model;
                        base.Model.Page.Errors = result.Errors;

                        return View["Register", base.Model];
                    }

                    var userGUID = UserMapper.ValidateRegisterNewUser(model);
                    DateTime? expiry = DateTime.Now.AddDays(7);

                    return this.LoginAndRedirect(userGUID.Value, expiry);
                };
        }
    }
}