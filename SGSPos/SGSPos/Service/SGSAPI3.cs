using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SGSPos.Service
{
    public class SGSAPI3
    {
        public static string baseUri = "https://codere-api.shoutz.com";

        public static async Task<User> TryLogin(string username, string password)
        {
            TryLoginRequest r = new TryLoginRequest()
            {
                username = username,
                userpassword = password,
                posid = "pos123",
            };
            TryLoginResponse response = await SGSAPI2.GenericPost<TryLoginResponse, TryLoginRequest>(baseUri + "/posLogin", r);

            if (response.result == "success")
                MessageBox.Show("Successfully logged in.");
            else
                MessageBox.Show("Logged in failed:\n" + response.error.message);

            User u = new User()
            {
                Username = username,
                Pulid = response.pulid,
                Permissions = new PermissionGroup()
                {
                    CanApprovePayout = int2Bool(response.rolepermissions.canapprovepayout),
                    CanApproveTotal = int2Bool(response.rolepermissions.canapprovetotal),
                    CanCreatePos = int2Bool(response.rolepermissions.cancreatepos),
                    CanDeactivatePos = int2Bool(response.rolepermissions.candeactivatepos),
                    CanManageUsers = int2Bool(response.rolepermissions.canmanageusers),
                    MaxPayout = response.rolepermissions.maxpayout,
                    Name = response.role
                }
            };

            return u;
        }

        public static async Task TryLogout(User u)
        {
            TryLogoutRequest r = new TryLogoutRequest()
            {
                pulid = u.Pulid
            };
            TryLogoutResponse response = await SGSAPI2.GenericPost<TryLogoutResponse, TryLogoutRequest>(baseUri + "/posLogout", r);
        }

        public static async Task PostCashOut(decimal amount, string date, User u)
        {

        }

        public static async Task PostCashIn(decimal amoumt, string date, User u)
        {

        }

        public static bool int2Bool(int i)
        {
            if (i <= 0) return false;
            else return true;
        }
    }

    public class SGSApi3Response
    {
        public bool success;
        public SGSApi3ErrorResponse error;
    }

    public class SGSApi3ErrorResponse
    {
        public string code;
        public string message;
    }

    public class TryLogoutRequest
    {
        public string pulid { get; set; }
    }

    public class TryLogoutResponse : SGSApi3Response
    {
        public string result { get; set; }
        public int finalDrawer { get; set; }
    }

    public class TryLoginRequest
    {
        public string userpassword { get; set; }
        public string username { get; set; }
        public string posid { get; set; }
    }

    public class TryLoginResponse : SGSApi3Response
    {
        public string result;
        public string role;
        public string pulid;
        public TryLoginPermissionResponse rolepermissions;
    }

    public class TryLoginPermissionResponse
    {
        public decimal maxpayout;
        public int canapprovepayout;
        public int canapprovetotal;
        public int canmanageusers;
        public int candeactivatepos;
        public int cancreatepos;

    }
}
