using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace SignalRTest.Pages
{
    public class StudentModel : PageModel
    {
        private readonly ILogger<StudentModel> _logger;

        public StudentModel(ILogger<StudentModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
