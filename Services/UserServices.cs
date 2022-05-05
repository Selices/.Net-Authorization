using Services.InterFace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserServices : IUserServices
    {
        public Task<string> GetUserId(string Id)
        {
            return Task.FromResult(Id);
        }
    }
}
