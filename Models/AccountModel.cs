using System.Data.SqlClient;
using System.Linq;

namespace QLNS.Models
{
    public class AccountModel
    {
        private readonly CSDLEntities context = null;
        public AccountModel()
        {
            context = new CSDLEntities();
        }
        //Lay quyen cua tai khoan
        public string GetPermission(string userName)
        {
            var sql = from s in context.TAI_KHOAN where s.USERNAME == userName select s.QUYEN;
            return sql.FirstOrDefault();
        }
        //Kiem tra dang nhap
        public bool Login(string Username, string Password)
        {
            object[] sqlParas = {
                new SqlParameter("@Username", Username),
                new SqlParameter("@Password", Password),
            };

            var res = context.Database.SqlQuery<bool>("Sp_Login @Username, @Password", sqlParas).SingleOrDefault();
            return res;
        }
    }
}