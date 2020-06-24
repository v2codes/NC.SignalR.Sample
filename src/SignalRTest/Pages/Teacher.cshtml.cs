using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SignalRTest.Pages
{
    public class TeacherModel : PageModel
    {
        private readonly ILogger<TeacherModel> _logger;

        public TeacherModel(ILogger<TeacherModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
