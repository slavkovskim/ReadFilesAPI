using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReadFilesAPI.Models;
using System.Collections;
using System.Text;
using System.Text.Json;

namespace ReadFilesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private const string Type = "image/jpeg";
        private const string FileName = "apple_logo.jpg";

        private const string Txt = "txt";
        private const string Txtname = "name_txt";

        private readonly DataContext _context;
        public FilesController(DataContext context)
        {
            _context = context;
        }

        //Get image as byte array
        [HttpGet("images-byte")]
        public IActionResult ReturnByteArray()
        {
            var bytes = Convert.FromBase64String(Models.Image.Base64Encoded);    

            return File(bytes, Type, FileName);
        }

        //Get image as stream
        [HttpGet("images-stream")]
        public IActionResult ReturnStream()
        {
            var stream = new MemoryStream(Convert.FromBase64String(Models.Image.Base64Encoded));

            return File(stream, Type, FileName);
        }


        //Shows list of files
        [HttpGet("get-files")] 
        public async Task<ActionResult<IEnumerable<FileDTO>>> GetFiles()
        {

            return await _context.Files.ToListAsync();

        }

        //Opens txt file
        [HttpGet("{id}")]
        public async Task<ActionResult<FileDTO>> GetFile(int id)
        {
            var file = await _context.Files.FindAsync(id);
            if (file == null)
            {
                return NotFound();
            }
            var path = file.Url;

            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var sr = new StreamReader(fs, Encoding.UTF8);

            string content = sr.ReadToEnd();

            var jsonString = JsonSerializer.Serialize(content).ToString();
   
            return file;
        }
    }
}