using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGSPos.Service
{
    public struct PermissionGroup
    {
        public decimal MaxPayout;
        public bool CanApprovePayout;
        public bool CanApproveTotal;
        public bool CanManageUsers;
        public bool CanDeactivatePos;
        public bool CanCreatePos;
        public string Name;
    }
}
