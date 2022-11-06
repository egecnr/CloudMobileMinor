using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExtraFunction.Authorization
{
    public static class AuthCheck
    {
        public static bool CheckIfUserNotAuthorized(FunctionContext Context)
        {
            ClaimsPrincipal user;
            if (Context.Items.TryGetValue("User", out object ReturnedUser))
                user = (ClaimsPrincipal)ReturnedUser;
            else
                user= null;

            return user == null;
        }
    }
}
