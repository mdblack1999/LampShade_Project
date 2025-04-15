using _01_LampShadeQuery.Contracts.Slide;
using ShopManagement.Infrastructure.EfCore;
using System.Collections.Generic;
using System.Linq;

namespace _01_LampShadeQuery.Query
{
    public class SlideQuery : ISlideQuery
    {
        private readonly ShopContext _shopContext;

        public SlideQuery(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        public List<SlideQueryModel> GetListSlides()
        {
            return _shopContext.Slides.Where(x => x.IsRemoved == false)
                .Select(x => new SlideQueryModel
                {
                    Title = x.Title ,
                    BtnText = x.BtnText ,
                    Heading = x.Heading ,
                    Link = x.Link ,
                    Picture = x.Picture ,
                    PictureAlt = x.PictureAlt ,
                    PictureTitle = x.PictureTitle ,
                    Text = x.Text
                }).ToList();

        }
    }
}
