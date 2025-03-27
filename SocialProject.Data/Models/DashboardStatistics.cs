using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialProject.Data.Models
{
    public class DashboardStatistics
    {
        public int TotalPosts { get; set; }
        public int TotalUsers { get; set; }
        public int TotalLikes { get; set; }
        public int TotalComments { get; set; }
        public int TotalFriendRequests { get; set; }
    }
}
