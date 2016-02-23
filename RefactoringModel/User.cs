namespace RefactoringModel
{
    using Newtonsoft.Json;
    using System;

    [Serializable]
    public class User
    {
        [JsonProperty("Username")]
        public string Name;
        [JsonProperty("Password")]
        public string Password;
        [JsonProperty("Balance")]
        public double Balance;
    }
}
