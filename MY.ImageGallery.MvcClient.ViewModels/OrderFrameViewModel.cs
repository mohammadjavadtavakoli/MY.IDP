namespace MY.ImageGallery.MvcClient.ViewModels
{
    public class OrderFrameViewModel
    {
        public string Address { get;  }=String.Empty;

        public OrderFrameViewModel(string address)
        {
            Address = address;
        }

    }
}

