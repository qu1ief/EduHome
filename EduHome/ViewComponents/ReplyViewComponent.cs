using Microsoft.AspNetCore.Mvc;

namespace EduHome.ViewComponents;

public class ReplyViewComponent:ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}
