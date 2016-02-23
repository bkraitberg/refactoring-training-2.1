namespace RefactoringModel
{
    using Newtonsoft.Json;
    using System;

    [Serializable]
    public class Product
    {
        [JsonProperty("Name")]
        public string Name;
        [JsonProperty("Price")]
        public double Price;
        [JsonProperty("Quantity")]
        public int Quantity;
    }
}
