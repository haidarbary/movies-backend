using DataAccess.Data;
using DataAccess.Models;

namespace BackendAPI.Controllers
{
    public class MovieApiResponse
    {
        public List<Movie>? Results { get; set; }
    }
}
