namespace mainMasterpiesce.Controllers
{
    internal class GraphServiceClient
    {
        private object authenticationProvider;

        public GraphServiceClient(object authenticationProvider)
        {
            this.authenticationProvider = authenticationProvider;
        }
    }
}