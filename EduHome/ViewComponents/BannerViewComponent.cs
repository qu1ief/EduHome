﻿using Microsoft.AspNetCore.Mvc;

namespace EduHome.ViewComponents;

public class BannerViewComponent:ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string name)
    {
        ViewBag.Name = name;

        return View();
    }
}
