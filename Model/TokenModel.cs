namespace AuthStudy.Model
{
    public class TokenModel
    {
        //设置密钥
        public string sercrt { get; set; }
        //颁发者
        public string issuer { get; set; }
        //接收者
        public string audience { get; set; }
    }
}
