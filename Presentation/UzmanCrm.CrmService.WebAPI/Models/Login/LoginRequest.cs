namespace UzmanCrm.CrmService.WebAPI.Models.Login
{
    /// <summary>
    /// Login istek modeli
    /// </summary>
    public class LoginRequest
    {
        //[SwaggerExclude]

        /// <summary>
        /// Sistem yöneticisi tarafından verilen api kullanıcı adı bilgisi. 
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Sistem yöneticisi tarafından verilen api parola bilgisi. 
        /// </summary>
        public string Password { get; set; }
    }
}