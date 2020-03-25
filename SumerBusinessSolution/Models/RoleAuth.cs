using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Models
{
    // Role Authorization 
    // To give authorization and access to users based on their role
    public class RoleAuth
    {
        public int Id { get; set; }

        // Role name = Admin, Supervisor or store user
        public string RoleName  { get; set; }

        // Approve Transfar Requests
        public bool AppTransReq { get; set; }

        // Create Transfer
        public bool CreateTrans { get; set; }

    }
}
