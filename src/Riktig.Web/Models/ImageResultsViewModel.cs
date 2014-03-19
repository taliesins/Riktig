namespace Riktig.Web.Models
{
    using System;
    using System.Collections.Generic;


    public class ImageResultsViewModel
    {
        readonly IEnumerable<Uri> _images;

        public ImageResultsViewModel(IEnumerable<Uri> images)
        {
            _images = images;
        }

        public IEnumerable<Uri> Images
        {
            get { return _images; }
        }
    }
}