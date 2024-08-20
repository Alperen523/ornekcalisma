namespace UzmanCrm.CrmService.Application.Abstractions.Service.ExampleService.Model.Procedure
{
    public class UsernameModel
    {
        public UsernameModel(string username)
        {
            this.UserName = username;
        }
        public string UserName { get; set; }
    }
}
