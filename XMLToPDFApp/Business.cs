namespace XMLToPDFApp
{
    class Business
    {
        public string id;
        public string name;
        public string addressLine1;
        public string addressLine2;
        public string addressLine3;
        public string logoName;

        public Business(string id, string name, string addressLine1, string addressLine2, string addressLine3, string logoName)
        {
            this.id = id;
            this.name = name;
            this.addressLine1 = addressLine1;
            this.addressLine2 = addressLine2;
            this.addressLine3 = addressLine3;
            this.logoName = logoName;
        }
    }
}
