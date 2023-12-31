using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PluginCore;

namespace AnkiPlus.Controllers
{
    /// <summary>
    /// 其实也可以不写这个, 直接访问 Plugins/AnkiPlus/index.html
    /// 
    /// 下面的方法, 是去掉 index.html
    /// 
    /// 若 wwwroot 下有其它需要访问的文件, 如何 css, js, 而你又不想每次新增 action 指定返回, 则 Route 必须 Plugins/{PluginId},
    /// 这样访问 Plugins/AnkiPlus/css/main.css 就会访问到你插件下的 wwwroot/css/main.css
    /// </summary>
    [Route($"api/Plugins/{(nameof(AnkiPlus))}")]
    [Authorize("PluginCore.Admin")]
    public class HomeController : Controller
    {
        [HttpGet]
        [Route($"/Plugins/{nameof(AnkiPlus)}")]
        public async Task<ActionResult> Index()
        {
            string indexFilePath = System.IO.Path.Combine(PluginPathProvider.PluginWwwRootDir(nameof(AnkiPlus)), "index.html");

            return PhysicalFile(indexFilePath, "text/html");
        }

        /// <summary>
        /// 转换单个 markdown 文件为多个笔记卡片
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(nameof(ConvertMd2Notes))]
        public async Task<ActionResult> ConvertMd2Notes([FromQuery] string mdFilePath)
        {
            try
            {
                Utils.AnkiConnectUtil ankiConnectUtil = new Utils.AnkiConnectUtil();

                await ankiConnectUtil.ConvertMd2Notes(mdFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return Ok("Ok");
        }

    }
}
