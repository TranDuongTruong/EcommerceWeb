using System.ComponentModel.DataAnnotations;

using AppMvc.Net.Models.Blog;

namespace AppMvc.Net.Areas.Blog.Models {
    public class CreatePostModel : Post {
        [Display(Name = "Chuyên mục")]
        public int[] CategoryIDs { get; set; }
    }
}