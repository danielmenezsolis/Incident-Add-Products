namespace Incident_Add_Products
{
    internal class BasicHttpBinding
    {
        private object transportWithMessageCredential;

        public BasicHttpBinding(object transportWithMessageCredential)
        {
            this.transportWithMessageCredential = transportWithMessageCredential;
        }
    }
}