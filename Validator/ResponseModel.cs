namespace trilha_net_api_desafio.Validator
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            IsValid = true;
            ValidationMessages = new List<string>();
        }
        public bool IsValid { get; set; }
        public List<string> ValidationMessages { get; set; }
    }
}
